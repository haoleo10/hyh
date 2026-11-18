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
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Model.Param;
using System.Text.RegularExpressions;
using YiSha.Model.SoketData;
using System.Text.Json;
using Org.BouncyCastle.Tls.Crypto;
using System.IO;

namespace YiSha.Service.SystemManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2024-12-17 15:20
    /// 描 述：动态数据组服务类
    /// </summary>
    public class DynamicObjectGroupService : RepositoryFactory
    {
        #region 获取数据
        public async Task<List<DynamicObjectGroupEntity>> GetList(DynamicObjectGroupListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<DynamicObjectGroupEntity>> GetPageList(DynamicObjectGroupListParam param, Pagination pagination)
        {
            pagination.Sort = "Subscript";
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression, pagination);
            return list.OrderBy(t => t.Subscript).ToList();
        }

        public async Task<DynamicObjectGroupEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<DynamicObjectGroupEntity>(id);
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(DynamicObjectGroupEntity entity)
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
            await this.BaseRepository().Delete<DynamicObjectGroupEntity>(idArr);
        }
        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task ImportJson(ImportParam param)
        {
            if (param.IsOverride == 1)
            {
                await this.BaseRepository().FindObject("DELETE FROM dynamic_object_group");
            }
            string OGDMES = "";
            string absoluteFilePath = GlobalContext.HostingEnvironment.ContentRootPath + param.FilePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            if (File.Exists(absoluteFilePath))
            {
                try
                {
                    using (StreamReader reader = new StreamReader(absoluteFilePath))
                    {
                        OGDMES = reader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("出现错误: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine($"文件 {absoluteFilePath} 不存在，请检查！");
                // 这里也可以根据实际需求考虑抛出一个自定义异常等更合适的错误处理方式
            }

            List<string> resultList = ExtractMatches(OGDMES, @"&([^;]+);");
            List<int> numberList = ExtractNumbers(OGDMES);
            List<string> resultListType = ExtractMatches(OGDMES, @"([^\s&]+)\s*&");
            List<string> charList = ExtractMatches(OGDMES, @"//(.*)");
            List<DynamicObjectGroupEntity> dynamicObjects = new List<DynamicObjectGroupEntity>();
            for (int i = 0; i < resultList.Count; i++)
            {
                DynamicObjectGroupEntity dynamicObjectGroupEntity = new DynamicObjectGroupEntity();
                await dynamicObjectGroupEntity.Create();
                dynamicObjectGroupEntity.ProtName = resultList[i];
                dynamicObjectGroupEntity.Subscript = numberList[i + 1];
                dynamicObjectGroupEntity.DataType = resultListType[i];
                if (i < charList.Count)
                {
                    dynamicObjectGroupEntity.Remake = charList[i];
                }           
                dynamicObjects.Add(dynamicObjectGroupEntity);
            }
            await this.BaseRepository().Insert(dynamicObjects.OrderBy(t => t.Subscript).ToList());
        }

        static List<string> ExtractMatches(string input, string pattern)
        {
            List<string> result = new List<string>();
            Regex regex = new Regex(pattern, RegexOptions.Compiled);
            MatchCollection matches = regex.Matches(input);
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                if (match.Groups.Count > 1)
                {
                    string extractedString = match.Groups[1].Value.Trim();
                    result.Add(extractedString);
                }
            }
            return result;
        }

        static List<int> ExtractNumbers(string input)
        {
            List<int> result = new List<int>();
            Regex regex = new Regex(@"\[(\d+)\]");
            MatchCollection matches = regex.Matches(input);
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                if (match.Groups.Count > 1)
                {
                    string extractedString = match.Groups[1].Value.Trim();
                    result.Add(int.Parse(extractedString));
                }
            }
            return result;
        }


        public async void DybanucData(string Jsondata)
        {
            //Jsondata = "{\"header\":{\"packet_mask\":11111111,\"trans_id\":1,\"has_checksum\":0,\"checksum_code\":0,\"packet_len\":520},\"body\":{\"DataType\":1,\"TotalSize\":520,\"PacketSerNo\":99,\"TimeStamp\":1764898087,\"FSS_Time\":1256050,\"SimState\":1,\"CurrentIC\":461,\"CommandID\":0,\"ResponseID\":0,\"ArrSize\":50,\"CmdSize\":0,\"ArrData\":[1,1,0,33.417,1,0,192215,294267,385518,101006,147271,178128,101966,528371,0,0,0.386701,0.999131,64.12,869.118,0.000619905,4.03203e-006,-206.496,2.8925e+009,0.000620144,-0.000206576,3.7754e+006,4.03359e-006,0.999517,1,1200,1000,11,25,0.999,0.12,0.1,0.02,0.22,0.1,0.1,5,1,1,2895,96.4728,99.6622,97.6599,224.59,224.764],\"CmdData\":\"\"}}";
            string pattern = @"^{""header"":.*}}";
            MatchCollection matches = Regex.Matches(Jsondata, pattern);
            if (matches.Count > 0)
            {
                Jsondata = matches[matches.Count - 1].Value;
                var expression = LinqExtensions.True<FSSocketEntity>();
                FSSocketEntity fSSocketEntity = this.BaseRepository().IQueryable(expression).FirstOrDefault();
                if (fSSocketEntity != null)
                {
                    fSSocketEntity.Jsons = Jsondata.Length > 5000 ? Jsondata.Substring(0, 5000) : Jsondata;
                    await fSSocketEntity.Modify();
                    await this.BaseRepository().Update(fSSocketEntity);
                }
                try
                {
                    SocketDataInfo dataObject = new SocketDataInfo();
                    // 使用System.Text.Json进行反序列化操作
                    dataObject = JsonSerializer.Deserialize<SocketDataInfo>(Jsondata);
                    if (dataObject != null)
                    {
                        #region 获取动态对象组  方法1  循环遍历速度慢
                        ////获取动态对象组
                        //var expressionDynamicObject = LinqExtensions.True<DynamicObjectGroupEntity>();
                        //List<DynamicObjectGroupEntity> dynamicObjectGroupEntities = this.BaseRepository().IQueryable(expressionDynamicObject).ToList();

                        //for (int i = 0; i < dataObject.body.ArrData.Count; i++)
                        //{
                        //    foreach (var item in dynamicObjectGroupEntities)
                        //    {
                        //        if (item.Subscript == i)
                        //        {
                        //            item.ProtVelue = dataObject.body.ArrData[i].ToString();
                        //            break;
                        //        }
                        //    }
                        //}
                        #endregion

                        #region 获取动态对象组 方法2 使用 LINQ 关联查询替代循环嵌套
                        //// 获取动态对象组 
                        //var expressionDynamicObject = LinqExtensions.True<DynamicObjectGroupEntity>();
                        //List<DynamicObjectGroupEntity> dynamicObjectGroupEntities = this.BaseRepository().IQueryable(expressionDynamicObject).ToList();

                        //var query = from entity in dynamicObjectGroupEntities
                        //            join data in dataObject.body.ArrData.Select((value, index) => new { Value = value, Index = index })
                        //            on entity.Subscript equals data.Index
                        //            select new { Entity = entity, Data = data.Value };

                        //foreach (var result in query)
                        //{
                        //    result.Entity.ProtVelue = result.Data.ToString();
                        //}
                        #endregion

                        #region 获取动态对象组 方法3 大批量操作理论速度最快的
                        // 获取动态对象组
                        var expressionDynamicObject = LinqExtensions.True<DynamicObjectGroupEntity>();
                        List<DynamicObjectGroupEntity> dynamicObjectGroupEntities = this.BaseRepository().IQueryable(expressionDynamicObject).ToList();

                        // 将列表转换为字典，以Subscript作为键，方便快速查找
                        Dictionary<int?, DynamicObjectGroupEntity> entityDictionary = dynamicObjectGroupEntities.ToDictionary(entity => entity.Subscript);

                        for (int i = 0; i < dataObject.body.ArrData.Count; i++)
                        {
                            if (entityDictionary.TryGetValue(i, out var item))
                            {
                                item.ProtVelue = dataObject.body.ArrData[i].ToString();
                            }
                        }
                        #endregion
                        await this.BaseRepository().Update(dynamicObjectGroupEntities);

                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex.Message, ex);
                }
            }


           

        }
        #endregion

        #region 私有方法
        private Expression<Func<DynamicObjectGroupEntity, bool>> ListFilter(DynamicObjectGroupListParam param)
        {
            var expression = LinqExtensions.True<DynamicObjectGroupEntity>();
            if (param != null)
            {
                expression = expression.And(t => t.ProtName.Contains(param.ProtName));
            }
            return expression;
        }
        #endregion
    }
}
