using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BasicKnowledge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //创建主机的代码位于 Program.Main 中，并遵循生成器模式。 调用方法来配置属于主机的每个资源。 调用生成器方法以拉取其所有内容并实例化该主机对象。
            CreateWebHostBuilder(args).Build().Run();

        }
        /*
         * ASP.NET Core 应用在启动时构建主机。 主机是封装所有应用资源的对象，例如：
         * HTTP 服务器实现
            中间件组件
            日志记录
            DI
            Configuration
         */
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();


    }
}
