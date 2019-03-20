using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.AspNetCore.Routing;

namespace HelloWorld
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup()
        {
            //加载AppSettings.json文件，构建配置项目
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("AppSettings.json");
            Configuration = builder.Build();
        }

        //配置服务
        public void ConfigureServices(IServiceCollection services)
        {
            //添加MVC服务，才能使用MVC的Controller
            services.AddMvc();
        }

        /*
         * 请求管道中的中间件
         * 中间件是一种装配到应用程序管道以处理请求和响应的组件
         * ASP.NET Core 中的中间件控制我们的应用程序如何响应 HTTP 请求，它还可以控制我们的应用程序在发生错误时的显示的内容，
         * 它是我们认证和授权用户执行特定操作的关键部分
         * 可以使用 Run、Map 和 Use 扩展方法来配置请求委托
         * 此外，委托还可以决定不将请求传递给下一个委托，这就是对请求管道进行短路
         * 每个新项目默认都会使用 context.Response.WriteAsync 中间件，也就是在 app.Run() 方法中注册的中间件
        */
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //这是表明当前是调试模式，不是生产模式，产生exception的话会调到exception页面
            if (env.IsDevelopment())
            {
                //使用UseDeveloperExceptionPage中间件, 这个中间件不关心传入的请求，因为总是在管道后发生
                //它会调用下一个中间件，然后等待是否异常，如果出现异常则返回错误页面
                app.UseDeveloperExceptionPage();
            }

            //UseFileServer是对UseDefaultFiles和UseStaticFiles的封装，搞不清楚UseDefaultFiles和UseStaticFiles顺序时用UseFileServer
            //静态文件放到wwwroot文件夹下面
            app.UseFileServer();

            //app.UseMvcWithDefaultRoute() 给了一个默认的路由规则，允许我们访问 HomeController
            //app.UseMvcWithDefaultRoute();
            app.UseMvc(ConfigureRoute);

            //UseDefaultFiles 中间件会检查传入的请求并检查它是否用于目录的根目录，以及是否有任何匹配的默认文件
            //可以自己重写UseDefaultFiles的默认文件index.html默认是是默认文件之一
            //app.UseDefaultFiles();
            //UseStaticFiles中间件会让请求寻找wwwroot下的静态文件，如果没有文件就进入下一个中间件
            //app.UseStaticFiles();


            //这也是个中间件
            //app.UseWelcomePage();

            /*
             * app.Run() 方法允许我们传入另一个方法，我们可以用它来处理每一个单独的响应。Run() 方法不经常见，它是调用中间件的终端
             * 在 app.Run() 方法中注册的中间件永远不会有机会调用另一个中间件，它只会接收请求，然后必须产生某种响应
             * app.Run() 方法中注册的中间件还可以访问 Response，例如使用 Response 对象返回一个字符串
             * 如果在 app.Run() 方法之后注册另一个中间件，那么注册的那个中间件永远不会被调用，因为 Run() 方法是注册中间件的终端，在它之后，永远不会调用下一个中间件
             */
            app.Run(async (context) =>
            {
                //throw new Exception("Throw Exception");

                //await context.Response.WriteAsync("Hello World! 简单教程");

                //利用了AppSettings.json文件中的值
                var msg = Configuration["message"];

                await context.Response.WriteAsync(msg);
            });
        }

        private void ConfigureRoute(IRouteBuilder routeBuilder)
        {
            //在这里配置路由，必须使用IRouteBuilder的参数

            // Home/Index
            //{controller=Home}/{action=Index}是设置默认控制器和Action，
            //如果访问的URL无法匹配到路由，则请求会进入下一个中间件而不会进入app.UseMvc(ConfigureRoute)
            routeBuilder.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
