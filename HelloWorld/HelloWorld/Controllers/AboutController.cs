using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld.Controllers
{
    //直接使用属性路由在Controller里面设置
    [Route("About")]
    public class AboutController
    {
        [Route("")]
       public string Phone()
        {
            return "+10086";
        }


        //[Route("Countrys")]
        //使用令牌控制器，即便Action名称变了也不用修改属性路由
        [Route("[action]")]
        public string Country()
        {
            return "中国";
        }
    }
}
