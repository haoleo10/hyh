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
    public class LossAnalysisController : BaseController
    {
        [Area("ThermalPerformanceManage")]
        public ActionResult LossAnalysisIndex()
        {
            return View();
        }
    }
}
