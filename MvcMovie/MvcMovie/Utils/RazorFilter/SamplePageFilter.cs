using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Utils.RazorFilter
{
    public class SamplePageFilter : IPageFilter
    {
        //Handle Method执行之后，Action Result之前。
        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            throw new NotImplementedException();
        }
        //Handle Method执行之前，模型绑定完成以后调用
        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            throw new NotImplementedException();
        }
        //Handle Method选择之后，模型绑定发生之前
        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
            throw new NotImplementedException();
        }
    }
}
