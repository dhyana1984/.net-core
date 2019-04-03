using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MvcMovie.Models;
using MvcMovie.Utils.RazorFilter;

namespace MvcMovie.Pages
{
    [AddHeader("Author", "Rick")]
    public class IndexModel : PageModel
    {
        private readonly ILogger _logger;
        private readonly MvcMovie.Models.MvcMovieContext _context;

        public IndexModel(MvcMovie.Models.MvcMovieContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }
        //在Razor页面中没有ViewBag
        //属性上加了ViewData，可以在_Layout中以ViewData["TestProperty"]的方式读取数据
        [ViewData]
        public string TestProperty { get; } = "Test";

        //[TempData] 是 ASP.NET Core 2.0 中的新属性，在控制器和页面上受支持
        //页面模型将 [TempData] 属性应用到 Message 属性。
        [TempData]
        public string Message { get; set; }

        public IList<Movie> Movies;
        public SelectList Genres;
        //[BindProperty] 特性来选择加入模型绑定。在页面上可以直接用此属性绑定，否则不会绑定
        // 会绑定名称与属性相同的表单值和查询字符串。 在 GET 请求中进行绑定需要 (SupportsGet = true)
        [BindProperty(SupportsGet =true)]
        public string MovieGenre { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
  
        [BindProperty]
        public MovieGenreViewModel MV { get; set; }
      
        public async Task OnGetAsync()
        {
            var movies = from m in _context.Movie select m;
            //检索后需要重新绑定Genres
            IQueryable<string> genreQuery = from m in _context.Movie orderby m.Genre select m.Genre;
            //根据关键字和电影流派检索
            if (!string.IsNullOrEmpty(SearchString))
            {
                movies = movies.Where(t => t.Title.Contains(SearchString));
            }
            if (!string.IsNullOrEmpty(MovieGenre))
            {
                movies = movies.Where(t => t.Genre == MovieGenre);
            }
            Movies = await movies.ToListAsync();
            
            Genres = new SelectList(await genreQuery.Distinct().ToListAsync());
        }

        public async Task  OnPostSearchMovieAsync()
        {
         
            var movies = from m in _context.Movie select m;
            
            //根据关键字和电影流派检索
            if (!string.IsNullOrEmpty(SearchString))
            {
                movies = movies.Where(t => t.Title.Contains(SearchString));
            }
            if (!string.IsNullOrEmpty(MovieGenre))
            {
                movies = movies.Where(t => t.Genre == MovieGenre);
            }
            Movies = await movies.ToListAsync();
            //检索后需要重新绑定Genres
            IQueryable<string> genreQuery = from m in _context.Movie orderby m.Genre select m.Genre;
            Genres = new SelectList(await genreQuery.Distinct().ToListAsync());


        }


        //通过重写筛选器方法实现 Razor 页面筛选器
        public override void OnPageHandlerSelected(
                                 PageHandlerSelectedContext context)
        {
            _logger.LogDebug("IndexModel/OnPageHandlerSelected");
        }

        public override void OnPageHandlerExecuting(
                                    PageHandlerExecutingContext context)
        {
            Message = "Message set in handler executing";
            _logger.LogDebug("IndexModel/OnPageHandlerExecuting");
        }


        public override void OnPageHandlerExecuted(
                                    PageHandlerExecutedContext context)
        {
            _logger.LogDebug("IndexModel/OnPageHandlerExecuted");
        }
    }
}
