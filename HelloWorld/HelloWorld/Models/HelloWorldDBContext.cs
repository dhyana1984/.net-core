using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace HelloWorld.Models
{
    public class HelloWorldDBContext : IdentityDbContext<User>
    {

        //public HelloWorldDBContext()
        //{

        //}


        /*
         * 第一种配置EF方法，需要在startup的service里面使用services.AddEntityFrameworkSqlite().AddDbContext<HelloWorldDBContext>(options => options.UseSqlite(Configuration["database:connection"]))
         * 用于默认数据库
         */
        //public HelloWorldDBContext(DbContextOptions<HelloWorldDBContext> options):base(options)
        //{

        //}

        /*
         * 第二种配置EF的办法，重写OnConfiguring(DbContextOptionsBuilder optionsBuilder)
         * 在Startup的Service里面写 services.AddEntityFrameworkSqlite().AddDbContext<HelloWorldDBContext>()
         * 用于特定数据库
         */
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = blogging.db");
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
