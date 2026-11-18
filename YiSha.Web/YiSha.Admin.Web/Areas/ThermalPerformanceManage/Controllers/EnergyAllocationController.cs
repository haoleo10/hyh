using YiSha.Util.Model;
using YiSha.Entity;
using YiSha.Model;
using YiSha.Admin.Web.Controllers;
using YiSha.Entity.ThermalPerformanceManage;
using YiSha.Business.ThermalPerformanceManage;
using YiSha.Model.Param.ThermalPerformanceManage;
using Microsoft.AspNetCore.Mvc;


namespace YiSha.Admin.Web.Areas.ThermalPerformanceManage.Controllers
{
    public class EnergyAllocationController : BaseController
    {
        /// <summary>
        /// 创 建：admin
        /// 日 期：2024-08-19 15:23
        /// 描 述：能量分配控制器类
        /// </summary>
        /// 
        [Area("ThermalPerformanceManage")]

        #region 视图功能
        [AuthorizeFilter("thermalperformance:energyallocation:view")]

        public ActionResult EnergyAllocationIndex()
        {
            return View();
        }
        #endregion
    }
}
