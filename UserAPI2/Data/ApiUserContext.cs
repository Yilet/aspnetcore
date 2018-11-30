using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAPI2.Models;

namespace UserAPI2.Data
{
    public class ApiUserContext:DbContext
    {

        public ApiUserContext(DbContextOptions<ApiUserContext> options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>().ToTable("Users").HasKey(a => a.Id);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AppUser> Users { get; set; }
    }
}
