using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BasicKnowledge
{
    public class Startup
    {

        private readonly IHostingEnvironment _env; //按环境配置服务。
        private readonly IConfiguration _config;    //读取配置。
        private readonly ILoggerFactory _loggerFactory; //在记录器中创建 Startup.ConfigureServices。

        public Startup(IHostingEnvironment env, IConfiguration config, ILoggerFactory loggerFactory)
        {
            _env = env;
            _config = config;
            _loggerFactory = loggerFactory;
        }


        //将配置（或注册）服务的代码添加到 Startup.ConfigureServices 方法中。 服务是应用使用的组件。 例如，Entity Framework Core 上下文对象是一项服务
        /*
         * 可选择性地包括 ConfigureServices 方法以配置应用的服务。 
         * 服务是一个提供应用功能的可重用组件。 
         * 在 ConfigureServices 中配置配置（也称为“注册”）并通过依存关系注入 (DI) 或 ApplicationServices 在整个应用中使用
         * 在 Configure 方法配置应用服务之前，由主机调用
         */
        public void ConfigureServices(IServiceCollection services)
        {
            var logger = _loggerFactory.CreateLogger<Startup>();

            if (_env.IsDevelopment())
            {
                // Development service configuration

                logger.LogInformation("Development environment");
            }
            else
            {
                // Non-development service configuration

                logger.LogInformation($"Environment: {_env.EnvironmentName}");
            }

            // Configuration is available during startup.
            // Examples:
            //   _config["key"]
            //   _config["subsection:suboption1"]
        }

        /* 将配置请求处理管道的代码添加到 Startup.Configure 方法中。 管道由一系列中间件组件组成。 
         * 例如，中间件可能处理对静态文件的请求或将 HTTP 请求重定向到 HTTPS。 
         * 每个中间件在 HttpContext 上执行异步操作，然后调用管道中的下一个中间件或终止请求
         * 注册中间件的顺序
         *  异常/错误处理
            HTTP 严格传输安全协议
            HTTPS 重定向
            静态文件服务器
            Cookie 策略实施
            身份验证
            会话
            MVC
         */
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            /*
             * 执行环境（例如“开发”、“暂存”和“生产”）是 ASP.NET Core 中的高级概念。 
             * 可以通过设置 ASPNETCORE_ENVIRONMENT 环境变量来指定运行应用的环境。 
             * ASP.NET Core 在应用启动时读取该环境变量，并将该值存储在 IHostingEnvironment 实现中。 
             * 可通过 DI 在应用的任何位置使用环境对象。
             * 使用 Use、Run 和 Map 配置 HTTP 管道。 
             * Use 方法可使管道短路（即不调用 next 请求委托）。 
             * Run 是一种约定，并且某些中间件组件可公开在管道末尾运行的 Run[Middleware] 方法。
             * Map 扩展用作约定来创建管道分支。 Map* 基于给定请求路径的匹配项来创建请求管道分支。 如果请求路径以给定路径开头，则执行分支。
             */
            if (env.IsDevelopment())
            {
                // When the app runs in the Development environment:
                //   Use the Developer Exception Page to report app runtime errors.
                //   Use the Database Error Page to report database runtime errors.
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                // When the app doesn't run in the Development environment:
                //   Enable the Exception Handler Middleware to catch exceptions
                //     thrown in the following middlewares.
                //   Use the HTTP Strict Transport Security Protocol (HSTS)
                //     Middleware.
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            // Use HTTPS Redirection Middleware to redirect HTTP requests to HTTPS.
            app.UseHttpsRedirection();

            // Return static files and end the pipeline.
            app.UseStaticFiles();

            // Use Cookie Policy Middleware to conform to EU General Data 
            // Protection Regulation (GDPR) regulations.
            app.UseCookiePolicy();

            // Authenticate before the user accesses secure resources.
            app.UseAuthentication();

            // If the app uses session state, call Session Middleware after Cookie 
            // Policy Middleware and before MVC Middleware.
            app.UseSession();

            // Add MVC to the request pipeline.
            app.UseMvc();
        }
    }

    /*
     * 在应用的 Configure 中间件管道的开头或末尾使用 IStartupFilter 来配置中间件。 
     * IStartupFilter 有助于确保中间件在应用请求处理管道的开始或结束时由库添加的中间件之前或之后运行。
     * 在请求管道中，每个 IStartupFilter 实现一个或多个中间件。 
     * 筛选器按照添加到服务容器的顺序调用。 筛选器可在将控件传递给下一个筛选器之前或之后添加中间件，从而附加到应用管道的开头或末尾
     */
    //public class RequestSetOptionsStartupFilter : IStartupFilter
    //{
    //    //IStartupFilter 实现单个方法（即 Configure），该方法接收并返回 Action<IApplicationBuilder>。 IApplicationBuilder 定义用于配置应用请求管道的类。
    //    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    //    {
    //        return builder =>
    //        {
    //            builder.UseMiddleware<RequestSetOptionsMiddleware>();
    //            next(builder);
    //        };
    //    }
    //}

    //public class RequestSetOptionsMiddleware
    //{
    //    private readonly RequestDelegate _next;
    //    private IOptions<AppOptions> _injectedOptions;

    //    public RequestSetOptionsMiddleware(
    //        RequestDelegate next, IOptions<AppOptions> injectedOptions)
    //    {
    //        _next = next;
    //        _injectedOptions = injectedOptions;
    //    }

    //    public async Task Invoke(HttpContext httpContext)
    //    {
    //        Console.WriteLine("RequestSetOptionsMiddleware.Invoke");

    //        var option = httpContext.Request.Query["option"];

    //        if (!string.IsNullOrWhiteSpace(option))
    //        {
    //            _injectedOptions.Value.Option = WebUtility.HtmlEncode(option);
    //        }

    //        await _next(httpContext);
    //    }
    //}
}
