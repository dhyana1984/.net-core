using HelloWorld.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {

        }

        //public ObjectResult Index()
        //{
        //    var employee = new Employee() { ID = 1, Name = "张三" };
        //    //return Content("Hello Word! 此消息来自HomeController");
        //    //返回ObjectResult时,Json是默认格式
        //    return new ObjectResult(employee);
        //}

        public ViewResult Index()
        {
            var employee = new Employee() { ID = 1, Name = "张三" };
            return View(employee);
        }
    }
}
