using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Business.SystemManage;
using YiSha.Entity.SystemManage;
using YiSha.Util.Model;

namespace YiSha.Admin.WebApi.Controllers
{
    /// <summary>
    /// 3K数据接口
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    [AuthorizeFilter]
    public class _3KDataController : ControllerBase
    {
        private FSSocketBLL socketBLL = new FSSocketBLL();

        /// <summary>
        /// 获取3K数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<TData<FSSocketEntity>> GetAllData()
        {
            TData<FSSocketEntity> obj = await socketBLL.GetEntity();
            return obj;
        }

        /// <summary>
        /// 获取通信单点信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<TData<DynamicObjectGroupEntity>> Get3KData([FromQuery] string Name)
        {
            TData<DynamicObjectGroupEntity> obj = await socketBLL.GetDynamicObjectGroup(Name);
            return obj;
        }

        /// <summary>
        /// 获取通信多点信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<TData<List<DynamicObjectGroupEntity>>> Get3KDatas([FromQuery] string Name)
        {
            TData<List<DynamicObjectGroupEntity>> obj = await socketBLL.GetDynamicObjectGroups(Name);
            return obj;
        }

    }
}
