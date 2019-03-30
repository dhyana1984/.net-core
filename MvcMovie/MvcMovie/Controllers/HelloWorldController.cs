using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;



namespace MvcMovie.Controllers
{
    public class HelloWorldController : Controller
    {
      
        public async Task<IActionResult> Index()
        {
            ViewBag.Test = "abc";
            var result = await GetData();
            ViewBag.Test = result;
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

        private async Task<string> GetData()
        {
            await Task.Delay(5000);
            return "bbbbbb";
        }
    }
}
