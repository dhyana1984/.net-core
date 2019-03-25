using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;



namespace MvcMovie.Controllers
{
    public class HelloWorldController : Controller
    {
      
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Welcome(string name, int numTimes=1)
        {
            //使用 HtmlEncoder.Default.Encode 防止恶意输入
            //return HtmlEncoder.Default.Encode($"Hello {name}, Numtime is: {numTimes}");

            ViewBag.Message = "Hello " + name;
            ViewBag.NumTimes = numTimes;
            return View();
        }
    }
}
