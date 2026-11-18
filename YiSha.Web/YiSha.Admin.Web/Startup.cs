using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YiSha.Util;
using YiSha.Util.Model;
using YiSha.Admin.Web.Controllers;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using YiSha.Service.SystemManage;
using YiSha.Model.SoketData;
using Org.BouncyCastle.Utilities.Net;
using NPOI.SS.Formula.Functions;

namespace YiSha.Admin.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            WebHostEnvironment = env;
            GlobalContext.LogWhenStart(env);
            GlobalContext.HostingEnvironment = env;
        }

        // 此方法由运行时调用。使用此方法向容器添加服务
        public void ConfigureServices(IServiceCollection services)
        {
            if (WebHostEnvironment.IsDevelopment())
            {
                services.AddRazorPages().AddRazorRuntimeCompilation();
            }
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
                options.ModelMetadataDetailsProviders.Add(new ModelBindingMetadataProvider());
            }).AddNewtonsoftJson(options =>
            {
                // 返回数据首字母不小写，CamelCasePropertyNamesContractResolver是小写
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });


            //services.AddTcpServer();  // 关键注入 

            services.AddMemoryCache();
            services.AddSession();
            services.AddHttpContextAccessor();

            services.AddOptions();




            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(GlobalContext.HostingEnvironment.ContentRootPath + Path.DirectorySeparatorChar + "DataProtection"));

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);  // 注册Encoding

            GlobalContext.SystemConfig = Configuration.GetSection("SystemConfig").Get<SystemConfig>();
            GlobalContext.Services = services;
            GlobalContext.Configuration = Configuration;
        }

        // 此方法由运行时调用.使用此方法配置HTTP请求管道。.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!string.IsNullOrEmpty(GlobalContext.SystemConfig.VirtualDirectory))
            {
                app.UsePathBase(new PathString(GlobalContext.SystemConfig.VirtualDirectory)); // 让 Pathbase 中间件成为第一个处理请求的中间件， 才能正确的模拟虚拟路径
            }
            if (WebHostEnvironment.IsDevelopment())
            {
                GlobalContext.SystemConfig.Debug = true;
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            string resource = Path.Combine(env.ContentRootPath, "Resource");
            FileHelper.CreateDirectory(resource);

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = GlobalContext.SetCacheControl
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/Resource",
                FileProvider = new PhysicalFileProvider(resource),
                OnPrepareResponse = GlobalContext.SetCacheControl
            });
            app.UseSession();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
            GlobalContext.ServiceProvider = app.ApplicationServices;
        }
    }

    public static class TcpServerExtensions
    {
        public static IServiceCollection AddTcpServer(this IServiceCollection services)
        {
            // 注册后台服务 
            //services.AddHostedService<TcpBackgroundService>();
            // 可选：全局信号量（如需跨服务共享）
            services.AddSingleton(new SemaphoreSlim(100));
            return services;
        }
    }


    //    private static DynamicObjectGroupService dynamicObjectGroupService = new DynamicObjectGroupService();
    //    private async Task HandleClient(TcpClient client)
    //    {

    //        try
    //        {
    //            // 获取一个数据流对象来进行读取和写入
    //            NetworkStream stream = client.GetStream();

    //            // 为读取数据而准备缓存
    //            Byte[] bytes = new Byte[1024 * 1024];
    //            String data = null;

    //            // 循环接收客户端所发送的所有数据
    //            while (true)
    //            {
    //                int bytesRead = await stream.ReadAsync(bytes, 0, bytes.Length);
    //                if (bytesRead == 0)
    //                {
    //                    break;
    //                }
    //                // 把数据字符转化成ASCII字符串
    //                data = System.Text.Encoding.ASCII.GetString(bytes, 0, bytesRead);
    //                if (data.Length < 50000)
    //                {

    //                    Task.Run(() => {
    //                        dynamicObjectGroupService.DybanucData(data);
    //                    });
    //                }
    //                else
    //                {
    //                    // 处理客户端所发送的数据
    //                    data = data.ToUpper();
    //                }
    //                // 处理客户端所发送的数据
    //                if (GlobalContext.SystemConfig.dsc)
    //                {
    //                    GlobalContext.SystemConfig.dsc = false;
    //                    try
    //                    {
    //                        //不指定IP发送数据
    //                        await SendClient(client, true);                          

    //                    }
    //                    catch(Exception ex)
    //                    {
    //                        try
    //                        {
    //                            //指定IP发送数据
    //                            await SendClient(client);
    //                        }
    //                        catch (Exception exx)
    //                        {
    //                            _logger.LogDebug("指定ip发送数据失败 Error handling client: {0}", exx);
    //                        }                         
    //                    }


    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogDebug("Error handling client: {0}", ex);
    //        }
    //        finally
    //        {
    //            // 释放连接信号量，允许接收新的客户端连接
    //            _connectionSemaphore.Release();
    //            // 关闭并且结束连接
    //            client.Close();
    //        }
    //    }

    //    /// <summary>
    //    /// 处理发送数据
    //    /// </summary>
    //    /// <param name="client"></param>
    //    /// <returns></returns>
    //    private async Task SendClient(TcpClient client,bool isReset = false)
    //    {
    //        _logger.LogDebug("不指定IP开始准备发送数据");
    //        // 获取客户端IP和端口
    //        if (client.Client.RemoteEndPoint is IPEndPoint remoteEndPoint)
    //        {
    //            string clientIp = remoteEndPoint.Address.ToString(); // 客户端IP地址
    //            int clientPort = remoteEndPoint.Port; // 客户端端口号

    //            // 可在此处记录或使用客户端信息
    //            _logger.LogDebug($"不指定IP发送数据客户端 {clientIp}:{clientPort} 发送数据");
    //        }

    //        //SNAP 复位IC
    //        if (GlobalContext.SystemConfig.cmd == 7)
    //        {
    //            try
    //            {
    //                // 获取一个数据流对象来进行读取和写入
    //                NetworkStream stream = client.GetStream();
    //                SocketDataPut socketDataPut = new SocketDataPut();
    //                Header header = new Header();
    //                header.packet_mask = 1111;
    //                header.trans_id = 0;
    //                header.has_checksum = 0;
    //                header.checksum_code = 0;
    //                header.packet_len = 100;
    //                BodyPut bodyPut = new BodyPut();
    //                bodyPut.DataType = 4;
    //                bodyPut.TotalSize = 100;
    //                bodyPut.PacketSerNo = 2;
    //                bodyPut.TimeStamp = 1;
    //                bodyPut.FSS_Time = 0;
    //                bodyPut.SimState = 0;
    //                bodyPut.CurrentIC = 203;
    //                bodyPut.CommandID = GlobalContext.SystemConfig.cmd;
    //                bodyPut.ResponseID = 0;
    //                bodyPut.PointName = GlobalContext.SystemConfig.PointName;
    //                bodyPut.PointValue = GlobalContext.SystemConfig.PointValue;
    //                socketDataPut.header = header;
    //                socketDataPut.bodyPut = bodyPut;
    //                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(socketDataPut);
    //                byte[] msg = Encoding.UTF8.GetBytes(jsonString);
    //                await stream.WriteAsync(msg, 0, msg.Length);
    //                GlobalContext.SystemConfig.dsc = false;
    //                _logger.LogDebug("SNAP 复位IC数据发送成功");

    //            }
    //            catch (Exception ex)
    //            {
    //                _logger.LogDebug("SNAP 复位IC数据发送失败");
    //            }
    //        }
    //        //RESET 复位IC
    //        else if (GlobalContext.SystemConfig.cmd == 6)
    //        {
    //            try
    //            {
    //                // 获取一个数据流对象来进行读取和写入
    //                NetworkStream stream = client.GetStream();
    //                SocketDataPut socketDataPut = new SocketDataPut();
    //                Header header = new Header();
    //                header.packet_mask = 1111;
    //                header.trans_id = 0;
    //                header.has_checksum = 0;
    //                header.checksum_code = 0;
    //                header.packet_len = 100;
    //                BodyPut bodyPut = new BodyPut();
    //                bodyPut.DataType = 1;
    //                bodyPut.TotalSize = 100;
    //                bodyPut.PacketSerNo = 1;
    //                bodyPut.TimeStamp = 1;
    //                bodyPut.FSS_Time = 60;
    //                bodyPut.SimState = 1;
    //                bodyPut.CurrentIC = 203;
    //                bodyPut.CommandID = GlobalContext.SystemConfig.cmd;
    //                bodyPut.ResponseID = 0;
    //                bodyPut.PointName = GlobalContext.SystemConfig.PointName;
    //                bodyPut.PointValue = GlobalContext.SystemConfig.PointValue;
    //                socketDataPut.header = header;
    //                socketDataPut.bodyPut = bodyPut;
    //                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(socketDataPut);
    //                byte[] msg = Encoding.UTF8.GetBytes(jsonString);
    //                await stream.WriteAsync(msg, 0, msg.Length);
    //                GlobalContext.SystemConfig.dsc = false;
    //                _logger.LogDebug("RESET 复位IC数据发送成功");


    //            }
    //            catch (Exception)
    //            {

    //                _logger.LogDebug("RESET 复位IC数据发送失败");
    //            }

    //        }
    //        else
    //        {
    //            try
    //            {
    //                // 获取一个数据流对象来进行读取和写入
    //                NetworkStream stream = client.GetStream();
    //                SocketDataPut socketDataPut = new SocketDataPut();
    //                Header header = new Header();
    //                header.packet_mask = 1111;
    //                header.trans_id = 0;
    //                header.has_checksum = 0;
    //                header.checksum_code = 0;
    //                header.packet_len = 100;
    //                BodyPut bodyPut = new BodyPut();
    //                bodyPut.DataType = 1;
    //                bodyPut.TotalSize = 100;
    //                bodyPut.PacketSerNo = 1;
    //                bodyPut.TimeStamp = 1;
    //                bodyPut.FSS_Time = 60;
    //                bodyPut.SimState = 1;
    //                bodyPut.CurrentIC = 80;
    //                bodyPut.CommandID = GlobalContext.SystemConfig.cmd;
    //                bodyPut.ResponseID = 0;
    //                bodyPut.PointName = GlobalContext.SystemConfig.PointName;
    //                bodyPut.PointValue = GlobalContext.SystemConfig.PointValue;
    //                socketDataPut.header = header;
    //                socketDataPut.bodyPut = bodyPut;
    //                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(socketDataPut);
    //                byte[] msg = Encoding.UTF8.GetBytes(jsonString);
    //                await stream.WriteAsync(msg, 0, msg.Length);
    //                GlobalContext.SystemConfig.dsc = false;
    //                _logger.LogDebug("基础数据发送成功");

    //            }
    //            catch (Exception)
    //            {
    //                _logger.LogDebug("基础数据发送失败");
    //            }


    //        }
    //    }

    //    private async Task SendClient(TcpClient client)
    //    {

    //        _logger.LogDebug("指定IP开始准备发送数据");
    //        if (client.Client.RemoteEndPoint is IPEndPoint remoteEndPoint)
    //        {
    //            string clientIp = remoteEndPoint.Address.ToString(); // 客户端IP地址
    //            int clientPort = remoteEndPoint.Port; // 客户端端口号

    //            _logger.LogDebug($"发数据的客户端 {clientIp}:{clientPort}");
    //            try
    //            {
    //                using (TcpClient remoteClient = new TcpClient())
    //                {
    //                    try 
    //                    {
    //                        _logger.LogDebug($"指定IP发送数据客户端 {GlobalContext.SystemConfig.KSFIP}:{GlobalContext.SystemConfig.KSFPort}");
    //                        // 连接到指定的IP和端口
    //                        await remoteClient.ConnectAsync(GlobalContext.SystemConfig.KSFIP, int.Parse(GlobalContext.SystemConfig.KSFPort));
    //                        _logger.LogDebug("连接成功");
    //                        using (NetworkStream remoteStream = remoteClient.GetStream())
    //                        {
    //                            _logger.LogDebug("开始发送数据");
    //                            if (GlobalContext.SystemConfig.cmd == 7)
    //                            {
    //                                SocketDataPut socketDataPut = new SocketDataPut();
    //                                Header header = new Header();
    //                                header.packet_mask = 1111;
    //                                header.trans_id = 0;
    //                                header.has_checksum = 0;
    //                                header.checksum_code = 0;
    //                                header.packet_len = 100;
    //                                BodyPut bodyPut = new BodyPut();
    //                                bodyPut.DataType = 4;
    //                                bodyPut.TotalSize = 100;
    //                                bodyPut.PacketSerNo = 2;
    //                                bodyPut.TimeStamp = 1;
    //                                bodyPut.FSS_Time = 0;
    //                                bodyPut.SimState = 0;
    //                                bodyPut.CurrentIC = 203;
    //                                bodyPut.CommandID = GlobalContext.SystemConfig.cmd;
    //                                bodyPut.ResponseID = 0;
    //                                bodyPut.PointName = GlobalContext.SystemConfig.PointName;
    //                                bodyPut.PointValue = GlobalContext.SystemConfig.PointValue;
    //                                socketDataPut.header = header;
    //                                socketDataPut.bodyPut = bodyPut;
    //                                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(socketDataPut);
    //                                byte[] msg = Encoding.UTF8.GetBytes(jsonString);
    //                                await remoteStream.WriteAsync(msg, 0, msg.Length);
    //                                GlobalContext.SystemConfig.dsc = false;
    //                                _logger.LogDebug("数据发送成功");                                   
    //                            }
    //                            else if (GlobalContext.SystemConfig.cmd == 6)
    //                            {
    //                                SocketDataPut socketDataPut = new SocketDataPut();
    //                                Header header = new Header();
    //                                header.packet_mask = 1111;
    //                                header.trans_id = 0;
    //                                header.has_checksum = 0;
    //                                header.checksum_code = 0;
    //                                header.packet_len = 100;
    //                                BodyPut bodyPut = new BodyPut();
    //                                bodyPut.DataType = 1;
    //                                bodyPut.TotalSize = 100;
    //                                bodyPut.PacketSerNo = 1;
    //                                bodyPut.TimeStamp = 1;
    //                                bodyPut.FSS_Time = 60;
    //                                bodyPut.SimState = 1;
    //                                bodyPut.CurrentIC = 203;
    //                                bodyPut.CommandID = GlobalContext.SystemConfig.cmd;
    //                                bodyPut.ResponseID = 0;
    //                                bodyPut.PointName = GlobalContext.SystemConfig.PointName;
    //                                bodyPut.PointValue = GlobalContext.SystemConfig.PointValue;
    //                                socketDataPut.header = header;
    //                                socketDataPut.bodyPut = bodyPut;
    //                                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(socketDataPut);
    //                                byte[] msg = Encoding.UTF8.GetBytes(jsonString);
    //                                await remoteStream.WriteAsync(msg, 0, msg.Length);
    //                                GlobalContext.SystemConfig.dsc = false;
    //                                _logger.LogDebug("数据发送成功");                                 
    //                            }
    //                            else
    //                            {
    //                                SocketDataPut socketDataPut = new SocketDataPut();
    //                                Header header = new Header();
    //                                header.packet_mask = 1111;
    //                                header.trans_id = 0;
    //                                header.has_checksum = 0;
    //                                header.checksum_code = 0;
    //                                header.packet_len = 100;
    //                                BodyPut bodyPut = new BodyPut();
    //                                bodyPut.DataType = 1;
    //                                bodyPut.TotalSize = 100;
    //                                bodyPut.PacketSerNo = 1;
    //                                bodyPut.TimeStamp = 1;
    //                                bodyPut.FSS_Time = 60;
    //                                bodyPut.SimState = 1;
    //                                bodyPut.CurrentIC = 80;
    //                                bodyPut.CommandID = GlobalContext.SystemConfig.cmd;
    //                                bodyPut.ResponseID = 0;
    //                                bodyPut.PointName = GlobalContext.SystemConfig.PointName;
    //                                bodyPut.PointValue = GlobalContext.SystemConfig.PointValue;
    //                                socketDataPut.header = header;
    //                                socketDataPut.bodyPut = bodyPut;
    //                                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(socketDataPut);
    //                                byte[] msg = Encoding.UTF8.GetBytes(jsonString);
    //                                await remoteStream.WriteAsync(msg, 0, msg.Length);
    //                                GlobalContext.SystemConfig.dsc = false;
    //                                _logger.LogDebug("数据发送成功");
    //                            }
    //                        }
    //                        GlobalContext.SystemConfig.dsc = false;
    //                        remoteClient.Close();
    //                    }
    //                    catch (Exception ex)
    //                    {
    //                        _logger.LogDebug("连接失败");
    //                    }                     
    //                    remoteClient.Close();
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                GlobalContext.SystemConfig.dsc = false;
    //                _logger.LogDebug(ex, " 数据发送失败");                   
    //            }
    //            finally
    //            {
    //                GlobalContext.SystemConfig.dsc = false;
    //                _connectionSemaphore.Release();
    //            }
    //        }
    //    }
    //}
}
