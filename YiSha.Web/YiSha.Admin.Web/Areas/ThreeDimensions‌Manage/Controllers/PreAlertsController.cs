using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using YiSha.Util;
using YiSha.Util.Model;
using YiSha.Entity;
using YiSha.Model;
using YiSha.Admin.Web.Controllers;
using YiSha.Entity.ThreeDimensions‌Manage;
using YiSha.Business.ThreeDimensions‌Manage;
using YiSha.Model.Param.ThreeDimensions‌Manage;

namespace YiSha.Admin.Web.Areas.ThreeDimensions‌Manage.Controllers
{
    /// <summary>
    /// 描 述：预报警列表控制器类
    /// </summary>
    [Area("ThreeDimensions‌Manage")]
    public class PreAlertsController : BaseController
    {
        private PreAlertsBLL preAlertsBLL = new PreAlertsBLL();

        #region 视图功能
        public ActionResult PreAlertsIndex()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        public async Task<ActionResult> GetListJson(ListParamAlertsListParam param, string AlarmLayer, string SYS)
        {
            TData<List<PreAlertsEntity>> obj = await preAlertsBLL.GetList(param, AlarmLayer, SYS);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetPageListJson(ListParamAlertsListParam param, Pagination pagination, string AlarmLayer, string SYS)
        {
            TData<List<PreAlertsEntity>> obj = await preAlertsBLL.GetPageList(param, pagination, AlarmLayer, SYS);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<PreAlertsEntity> obj = await preAlertsBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 确认报警：写入报警列表并标记预报警为已确认
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> ConfirmAlarm(long id)
        {
            TData obj = await preAlertsBLL.ConfirmAlarm(id);
            return Json(obj);
        }

        [HttpPost]
        public async Task<ActionResult> SaveFormJson(PreAlertsEntity entity)
        {
            TData<string> obj = await preAlertsBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await preAlertsBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
