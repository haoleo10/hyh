# 二层页面 `Range` 报错修复说明

日期：2026-11-24

## 现象

访问二层左侧/右侧视图页面（`/ThreeDimensions‌Manage/TweFloor/LeftView`、`RightView`）时，浏览器 Console 每秒刷出红色报错：

```
Uncaught TypeError: Cannot read properties of undefined (reading 'Range')
    at Object.success (LeftView:...)
    at checkCondition (LeftView:...)
    at renderConfigList (LeftView:...)
```

页面上未配置阈值的测点越多，报错越频繁（每个测点每秒一次），同时高亮/报警判断逻辑中断。

## 原因链条

1. 前端 `checkCondition()` 每秒对每个测点请求 `~/ThreeDimensions‌Manage/Param_Range/GetRangeEntity?dotName=xxx&Value=xxx`；
2. 服务端 `Param_RangeService.GetRangeEntity(dotName, Value)` 先按点名查阈值表（`sys_param_threshold`），`FirstOrDefault()` 对**没有阈值记录的测点返回 `null`**，下一行直接访问 `.Value` 抛出 `NullReferenceException`；此外 `int.Parse(Value)` 遇到前端 `parseFloat` 产生的小数（如 `0.5`）会抛 `FormatException`，RightView 不传 `Value` 参数时抛 `ArgumentNullException`；
3. 全局异常过滤器 `GlobalExceptionFilter` 把异常包装成**不含 `Data` 字段**的 JSON（只有 `Message`）返回，HTTP 状态仍是 200，jQuery 走 `success` 回调；
4. 前端判空写的是 `res.Data !== null`，漏掉了 `undefined`，继续执行 `res.Data.Range` 即抛出 `TypeError`。

## 修改内容

### 1. 服务端根因修复

`YiSha.Business/YiSha.Service/ThreeDimensions‌Manage/Param_RangeService.cs`（`GetRangeEntity(string dotName, string Value)`）

- 查不到阈值记录（`null`）时跳过阈值更新，不再空引用；
- `int.Parse(Value)` 改为 `int.TryParse(Value, out int thresholdValue)`，非法/小数/空值直接跳过更新；
- 跳过更新后继续正常返回量程数据（`Param_RangeEntity`）。

修改前后对比：

```csharp
// 修改前
if (param_ThresholdEntity.Value != int.Parse(Value))
{
    param_ThresholdEntity.Value = int.Parse(Value);
    await param_ThresholdEntity.Modify();
    await this.BaseRepository().Update(param_ThresholdEntity);
}

// 修改后
// 测点可能未配置阈值记录，或传入值不是整数（前端 parseFloat 可能带小数），两种情况都直接跳过阈值更新
if (param_ThresholdEntity != null && int.TryParse(Value, out int thresholdValue) && param_ThresholdEntity.Value != thresholdValue)
{
    param_ThresholdEntity.Value = thresholdValue;
    await param_ThresholdEntity.Modify();
    await this.BaseRepository().Update(param_ThresholdEntity);
}
```

### 2. 前端判空加固

- `YiSha.Web/YiSha.Admin.Web/Areas/ThreeDimensions‌Manage/Views/TweFloor/LeftView.cshtml`（约 186 行）：
  `res.Data !== null` 改为 `res.Data != null`，`undefined` 也走"未设置量程"分支；
- `YiSha.Web/YiSha.Admin.Web/Areas/ThreeDimensions‌Manage/Views/TweFloor/RightView.cshtml`（约 185 行）：同样修改。

## 生效方式

- **C# 改动**：需重新编译并重启对应站点（`GetRangeEntity` 在 5000 端口的 `YiSha.Admin.Web` 项目里）；
- **cshtml 改动**：刷新页面即可生效（强刷 Ctrl+F5 避免缓存）。

## 注意事项

- Windows 下编译前必须先停掉正在运行的站点进程，否则 DLL 被锁定，MSBuild 报 `文件被“.NET Host (PID)”锁定`，重试 10 次后编译失败；
- 5001 端口同一时间只能由一个进程占用：VS 调试启动和命令行 `dotnet YiSha.Admin.WebApi.dll --urls http://127.0.0.1:5001` 二选一，否则报 `Failed to bind to address ... address already in use`；
- 本修复只解决 `Range` 报错。页面"数据不动"与该报错无关，另有两个设计行为需知晓：
  - `DataType=0`（实时）：后端走 IoTDB `select last*` 查询，只有实时库持续写入新数据时页面数值才会变；
  - `DataType=1`（历史）：时间游标每秒 +1，播到 `EndTimeKNS`/`EndTime3K` 后定格在最后一帧，需改回 `StartTime` 重新打开页面才能重播；`Multiple` 参数目前在 LeftView 中未生效（恒为 1 倍速）。
