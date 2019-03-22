using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld.Models
{
    public class HelloWorldDBContext : DbContext
    {

        public HelloWorldDBContext()
        {

        }

        public HelloWorldDBContext(DbContextOptions<HelloWorldDBContext> options):base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
    }
}
