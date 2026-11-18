using System;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Data;
using YiSha.Data.Repository;
using YiSha.Entity.ResultAnalysisManage;
using YiSha.Model.Param.ResultAnalysisManage;
using System.Runtime.InteropServices;
using NPOI.SS.Formula.Functions;
using System.ComponentModel.DataAnnotations;
using NPOI.SS.Formula.PTG;
using System.Drawing.Drawing2D;
using Newtonsoft.Json;
using YiSha.Entity.SystemManage;

namespace YiSha.Service.ResultAnalysisManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2024-08-19 15:00
    /// 描 述：结果分析(状态初始化)服务类
    /// </summary>
    /// 

    public class SocketObj
    {
        public header header { get; set; }
        public body body { get; set; }
    }

    public class header
    {
        public long packet_mask { get; set; }
        public long trans_id { get; set; }
        public long has_checksum { get; set; }
        public long checksum_code { get; set; }
        public long packet_len { get; set; }
    }

    public class body
    {
        public long DataType { get; set; }
        public long TotalSize { get; set; }
        public long PacketSerNo { get; set; }
        public long TimeStamp { get; set; }
        public long FSS_Time { get; set; }
        public long SimState { get; set; }
        public long CurrentIC { get; set; }
        public long CommandID { get; set; }
        public long ResponseID { get; set; }
        public long ArrSize { get; set; }
        public long CmdSize { get; set; }
        public List<long> ArrData { get; set; }
        public string CmdData { get; set; }
    }

    public class AnalysisInitializationService : RepositoryFactory
    {    

        #region 获取数据
        public async Task<List<AnalysisInitializationEntity>> GetList(AnalysisInitializationListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }


        public async Task<List<AnalysisInitializationEntity>> GetPageList(AnalysisInitializationListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();

        }
        #region MyRegion

      

        // 定义与C++中一致的结构体
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct CKnsDataInfoWrapper
        {
            public string strPath;
            public string strTime;
            public string strValue;
        }


        #endregion

        public async Task<AnalysisInitializationEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<AnalysisInitializationEntity>(id);
        }

        public async Task TestCmd(int cmd)
        {

            if (GlobalContext.SystemConfig.dsc == false)
            {
                GlobalContext.SystemConfig.dsc = true;
                GlobalContext.SystemConfig.cmd = cmd;
            }          
        }

        public async Task TestCmdData()
        {
            if (GlobalContext.SystemConfig.dsc == false)
            {
                GlobalContext.SystemConfig.dsc = true;
                GlobalContext.SystemConfig.PointName= "DQHEEN8";
                GlobalContext.SystemConfig.PointValue = 1;
                GlobalContext.SystemConfig.cmd = 0;
            }
        }
        public async Task StartSyncCmdData()
        {
            if (GlobalContext.SystemConfig.dsc == false)
            {
                GlobalContext.SystemConfig.dsc = true;
                GlobalContext.SystemConfig.PointName = "dcs_syn_start_IN";
                GlobalContext.SystemConfig.PointValue = 1;
                GlobalContext.SystemConfig.cmd = 0;
            }
        }

        public async Task StopSyncCmdData()
        {
            if (GlobalContext.SystemConfig.dsc == false)
            {
                GlobalContext.SystemConfig.dsc = true;
                GlobalContext.SystemConfig.PointName = "dcs_syn_stop_IN";
                GlobalContext.SystemConfig.PointValue = 1;
                GlobalContext.SystemConfig.cmd = 0;
            }
        }

        public async Task DiversitySyncCmdData()
        {
            if (GlobalContext.SystemConfig.dsc == false)
            {
                GlobalContext.SystemConfig.dsc = true;
                GlobalContext.SystemConfig.PointName = "dcs_syn_AM_IN";
                GlobalContext.SystemConfig.PointValue = 1;
                GlobalContext.SystemConfig.cmd = 0;
            }
        }

        /// <summary>
        /// 开始初始化
        /// </summary>
        /// <returns></returns>
        public async Task InitializationStarted()
        {
            if (GlobalContext.SystemConfig.dsc == false)
            {
                GlobalContext.SystemConfig.dsc = true;
                GlobalContext.SystemConfig.PointName = GlobalContext.SystemConfig.InitializationStarted;
                GlobalContext.SystemConfig.PointValue = 1;
                GlobalContext.SystemConfig.cmd = 0;
            }
        }

        /// <summary>
        /// 暂停初始化
        /// </summary>
        /// <returns></returns>
        public async Task InitializationPause()
        {
            if (GlobalContext.SystemConfig.dsc == false)
            {
                GlobalContext.SystemConfig.dsc = true;
                GlobalContext.SystemConfig.PointName = GlobalContext.SystemConfig.InitializationPause;
                GlobalContext.SystemConfig.PointValue = 1;
                GlobalContext.SystemConfig.cmd = 0;
            }
        }


        /// <summary>
        /// 终止初始化
        /// </summary>
        /// <returns></returns>
        public async Task InitializationTermination()
        {
            if (GlobalContext.SystemConfig.dsc == false)
            {
                GlobalContext.SystemConfig.dsc = true;
                GlobalContext.SystemConfig.PointName = GlobalContext.SystemConfig.InitializationTermination;
                GlobalContext.SystemConfig.PointValue = 1;
                GlobalContext.SystemConfig.cmd = 0;
            }
        }

        /// <summary>
        /// 加载IC
        /// </summary>
        /// <returns></returns>
        public async Task LoadingIC()
        {
            if (GlobalContext.SystemConfig.dsc == false)
            {
                GlobalContext.SystemConfig.dsc = true;
                GlobalContext.SystemConfig.PointName = GlobalContext.SystemConfig.LoadingIC;
                GlobalContext.SystemConfig.PointValue = 1;
                GlobalContext.SystemConfig.cmd = 0;
            }
        }


        /// <summary>
        /// 数据同步
        /// </summary>
        /// <returns></returns>
        public async Task DataSynchronization()
        {
            if (GlobalContext.SystemConfig.dsc == false)
            {
                GlobalContext.SystemConfig.dsc = true;
                GlobalContext.SystemConfig.PointName = GlobalContext.SystemConfig.DataSynchronization;
                GlobalContext.SystemConfig.PointValue = 1;
                GlobalContext.SystemConfig.cmd = 0;
            }
        }


        #endregion

        #region 提交数据
        public async Task SaveForm(AnalysisInitializationEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                await entity.Create();
                await this.BaseRepository().Insert(entity);
            }
            else
            {
                await entity.Modify();
                await this.BaseRepository().Update(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<AnalysisInitializationEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<AnalysisInitializationEntity, bool>> ListFilter(AnalysisInitializationListParam param)
        {
            var expression = LinqExtensions.True<AnalysisInitializationEntity>();
            if (param != null)
            {
            }
            return expression;
        }
        #endregion
    }
}
