using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiSha.Enum
{
    public enum StatusEnum
    {
        [Description("启用")]
        Yes = 1,

        [Description("禁用")]
        No = 0
    }

    public enum IsEnum
    {
        [Description("是")]
        Yes = 1,

        [Description("否")]
        No = 0
    }

    public enum NeedEnum
    {
        [Description("不需要")]
        NotNeed = 0,

        [Description("需要")]
        Need = 1
    }

    public enum OperateStatusEnum
    {
        [Description("失败")]
        Fail = 0,

        [Description("成功")]
        Success = 1
    }

    public enum UploadFileType
    {
        [Description("头像")]
        Portrait = 1,

        [Description("新闻图片")]
        News = 2,

        [Description("导入的文件")]
        Import = 10
    }

    public enum PlatformEnum
    {
        [Description("Web后台")]
        Web = 1,

        [Description("WebApi")]
        WebApi = 2
    }

    public enum PayStatusEnum
    {
        [Description("未知")]
        Unknown = 0,

        [Description("已支付")]
        Success = 1,

        [Description("转入退款")]
        Refund = 2,

        [Description("未支付")]
        NotPay = 3,

        [Description("已关闭")]
        Closed = 4,

        [Description("已撤销（付款码支付）")]
        Revoked = 5,

        [Description("用户支付中（付款码支付）")]
        UserPaying = 6,

        [Description("支付失败(其他原因，如银行返回失败)")]
        PayError = 7
    }

    public enum SimState
    {
        [Description("停止，前端传递给3k的消息置0")]
        ST_STOP = 0,

        [Description("运行")]
        ST_RUN = 1,

        [Description("冻结")]
        ST_FREEZE = 2,

        [Description("ST_FAIL")]
        ST_FAIL = 3,

        [Description("步进")]
        ST_STEP = 4,

        [Description("加载")]
        ST_LOAD = 5,

        [Description("加载数据")]
        ST_LOADING_DATA = 6,

        [Description("加载工具")]
        ST_LOADING_TASKS = 7,

        [Description("ST_RESET")]
        ST_RESET = 8,

        [Description("ST_SNAP")]
        ST_SNAP = 9,

        [Description("ST_BUILD")]
        ST_BUILD = 10,

        [Description("ST_RELOAD")]
        ST_RELOAD = 11,

        [Description("ST_SWCHECK")]
        ST_SWCHECK = 12,

        [Description("ST_RESETBACKTRACK")]
        ST_RESETBACKTRACK = 13,

        [Description("ST_BREAK")]
        ST_BREAK = 14,

        [Description("ST_BREAK_FREEZE")]
        ST_BREAK_FREEZE = 15,

        [Description("ST_SESSION_IO")]
        ST_SESSION_IO = 16,
    }

    public enum COMMANDID
    {
        [Description("非命令数据")]
        NULL = 0,

        [Description("步进")]
        STEP = 1,

        [Description("加载")]
        LOAD = 2,

        [Description("卸载")]
        UNLOAD = 3,

        [Description("运行")]
        RUN = 4,

        [Description("冻结")]
        FREEZE = 5,

        [Description("状态初始化模块_开始初始化")]
        SIM_SI = 6,

        [Description("状态初始化模块_初始化暂停")]
        SIM_IP = 7,

        [Description("状态初始化模块_初始化终止")]
        SIM_IT = 8,

        [Description("状态初始化模块_加载IC")]
        SIM_LOAD_IC = 9,

        [Description("热力性能模块_开始计算")]
        TPM_SC = 10,

        [Description("特征量计算模块_开始反算")]
        FCM_SRC = 11,

        [Description("特征量计算模块_停止反算")]
        FCM_S = 12,

        [Description("特征量计算模块_特征量导入模型")]
        FCM_IM = 13,
    }
}
