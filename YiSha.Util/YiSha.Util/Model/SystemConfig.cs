namespace YiSha.Util.Model
{
    public class SystemConfig
    {
        public SystemConfig()
        {
            DBSlowSqlLogTime = 5;
        }

        /// <summary>
        /// 是否是Demo模式
        /// </summary>
        public bool Demo { get; set; }

        /// <summary>
        /// 是否是调试模式
        /// </summary>
        public bool Debug { get; set; }

        /// <summary>
        /// 允许一个用户在多个电脑同时登录
        /// </summary>
        public bool LoginMultiple { get; set; }

        public string LoginProvider { get; set; }

        /// <summary>
        /// Snow Flake Worker Id
        /// </summary>
        public int SnowFlakeWorkerId { get; set; }

        /// <summary>
        /// api地址
        /// </summary>
        public string ApiSite { get; set; }

        /// <summary>
        /// 允许跨域的站点
        /// </summary>
        public string AllowCorsSite { get; set; }

        /// <summary>
        /// 网站虚拟目录
        /// </summary>
        public string VirtualDirectory { get; set; }

        public string DBProvider { get; set; }

        public string DBConnectionString { get; set; }

        /// <summary>
        ///  数据库超时间（秒）
        /// </summary>
        public int DBCommandTimeout { get; set; }

        /// <summary>
        /// 慢查询记录Sql(秒),保存到文件以便分析
        /// </summary>
        public int DBSlowSqlLogTime { get; set; }

        /// <summary>
        /// 数据库备份路径
        /// </summary>
        public string DBBackup { get; set; }

        public string CacheProvider { get; set; }

        public string RedisConnectionString { get; set; }

        /// <summary>
        /// KS 通信
        /// </summary>
        public string KSIP { get; set; }
        public string KSPort { get; set; }

        public string KSFIP { get; set; }
        public string KSFPort { get; set; }


        public string WebApiIP { get; set; }
        public string WebApiPort { get; set; }

        /// <summary>
        /// 控制是否传输命令
        /// </summary>
        public bool dsc { get; set; }

        public int cmd { get; set; }

        public string PointName { get; set; }
        public double PointValue { get; set; }

        public int CurrentIC { get; set; }

        public string IotDBURL { get; set; }

        public string IotDBURLNew{ get; set; }

        public string Authorization {get;set;}
        public string AuthorizationValue { get; set; }
        /// <summary>
        /// 服务器本地IP
        /// </summary>
        public string localIP { get; set; }


        #region 命令

        /// <summary>
        /// 开始反算
        /// </summary>
        public string StartCompute { get; set; }

        /// <summary>
        /// 停止反算
        /// </summary>
        public string StopCompute { get; set; }

        /// <summary>
        /// 开始初始化
        /// </summary>
        public string InitializationStarted { get; set; }

        /// <summary>
        /// 初始化暂停
        /// </summary>
        public string InitializationPause { get; set; }

        /// <summary>
        /// 初始化终止
        /// </summary>
        public string InitializationTermination { get; set; }

        /// <summary>
        /// 加载IC
        /// </summary>
        public string LoadingIC { get; set; }

        /// <summary>
        /// 数据同步
        /// </summary>
        public string DataSynchronization { get; set; }

        /// <summary>
        /// 二层左边接口
        /// </summary>
        public string ThreeLeftURL { get; set; }

        /// <summary>
        /// 报警接口
        /// </summary>
        public string API_BJADD { get; set; }

        /// <summary>
        /// 二层右边接口
        /// </summary>
        public string ThreeRightURL { get; set; }

        /// <summary>
        /// 二层右边接口
        /// </summary>
        public string ThreeThreeRightURL { get; set; }

        #endregion



    }
}