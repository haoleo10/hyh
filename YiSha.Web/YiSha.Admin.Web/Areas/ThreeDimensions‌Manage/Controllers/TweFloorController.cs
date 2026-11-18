using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using YiSha.Admin.Web.Controllers;
using YiSha.Business.ThreeDimensions‌Manage;
using YiSha.Entity;
using YiSha.Entity.ThreeDimensions‌Manage;
using YiSha.Model;
using YiSha.Model.IotDBData;
using YiSha.Model.Param.ThreeDimensions‌Manage;
using YiSha.Util;
using YiSha.Util.Model;

namespace YiSha.Admin.Web.Areas.ThreeDimensions‌Manage.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2026-05-07 15:17
    /// 描 述：二层界面控制器类
    /// </summary>
    [Area("ThreeDimensions‌Manage")]
    public class TweFloorController :  BaseController
    {
        private TweFloorBLL tweFloorBLL = new TweFloorBLL();

        #region 视图功能
        [AuthorizeFilter("threedimensions‌:twefloor:view")]
        public ActionResult TweFloorIndex()
        {
            return View();
        }

        public ActionResult TweFloorForm()
        {
            return View();
        }

        public ActionResult CenterView()
        {
            return View();
        }
        public ActionResult LeftView()
        {
            return View();
        }

        public ActionResult RightView()
        {
            return View();
        }

        public ActionResult ThreeRightView()
        {
            return View();
        }




        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("threedimensions‌:twefloor:search")]
        public async Task<ActionResult> GetListJson(TweFloorListParam param)
        {
            TData<List<TweFloorEntity>> obj = await tweFloorBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("threedimensions‌:twefloor:search")]
        public async Task<ActionResult> GetPageListJson(TweFloorListParam param, Pagination pagination)
        {
            TData<List<TweFloorEntity>> obj = await tweFloorBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<TweFloorEntity> obj = await tweFloorBLL.GetEntity(id);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetLeftData(string Name)
        {
            IotDBinfo iotDBResultDatas = new IotDBinfo();
            string url = "http://" + @GlobalContext.SystemConfig.WebApiIP + ":" + @GlobalContext.SystemConfig.WebApiPort + $"/IotDBData/GetIOTDBThreeSyetem?SystemName=ABP";
            //接收数据
            string responseBody = "";

            using (var client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        responseBody = await response.Content.ReadAsStringAsync();
                        if (!string.IsNullOrWhiteSpace(responseBody))
                        {
                            iotDBResultDatas = JsonConvert.DeserializeObject<IotDBinfo>(responseBody);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"请求失败，状态码：{(int)response.StatusCode} {response.ReasonPhrase}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"网络请求异常：{ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"发生异常：{ex.Message}");
                }
            }

            string pattern = @"(?<=\.)([A-Z0-9_]+)(?=\.value)";
            List<TweFloorEntity> tweFloorEntities = new List<TweFloorEntity>();
            for (int i = 0; i < iotDBResultDatas.values[0][0].Count; i++)
            {
                TweFloorEntity tweFloorEntity = new TweFloorEntity();
                tweFloorEntity.Id = i;
                tweFloorEntity.NamePort = Regex.Match(iotDBResultDatas.values[0][0][i] as string, pattern).Groups[1].Value ?? Regex.Match(iotDBResultDatas.values[0][0][i].ToString(), pattern).Groups[1].Value;
                tweFloorEntity.KNS = iotDBResultDatas.values[0][1][i];
                tweFloorEntity.RealTime = iotDBResultDatas.values[0][2][i];
                tweFloorEntities.Add(tweFloorEntity);
            }
            TData<List<TweFloorEntity>> data =new TData<List<TweFloorEntity>>();
            data.Data = tweFloorEntities;
            data.Tag = 1;
            return Json(data);


            //TData<TweFloorEntity> obj = await tweFloorBLL.GetEntity(id);
            //return Json(obj);
        }


        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("threedimensions‌:twefloor:add,threedimensions‌:twefloor:edit")]
        public async Task<ActionResult> SaveFormJson(TweFloorEntity entity)
        {
            TData<string> obj = await tweFloorBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("threedimensions‌:twefloor:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await tweFloorBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
