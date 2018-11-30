using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UserAPI2.Data;
using Microsoft.EntityFrameworkCore;

namespace UserAPI2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //ef core添加到依赖注入
            services.AddDbContext<ApiUserContext>(options =>
            {
                options.UseMySQL(Configuration.GetConnectionString("MysqlUser"));
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            UserContextSeed.SeedAsync(app, loggerFactory).Wait();
            //InitUserDatabase(app);
        }

        #region 1.0 初始化Users表的数据
        /// <summary>
        /// 1.0 初始化Users表的数据
        /// </summary>
        /// <param name="app"></param>
        public void InitUserDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())//1.0 根据IApplicationBuilder 获取scope
            {
                var userContext = scope.ServiceProvider.GetRequiredService<ApiUserContext>();//根据  scope和UserContext获取当前数据库的DbContext.
                var anyUser = userContext.Users;
                if (anyUser.Count() <= 0)
                {
                    userContext.Users.Add(new Models.AppUser { Name = "guandex" });
                    userContext.SaveChanges();
                }
                //userContext.Database.Migrate();
                //if (!userContext.Users.Any())//判断Users表是否有数据
                //{
                //    userContext.Users.Add(new Models.AppUser { Name = "guandex" });
                //    userContext.SaveChanges();
                //}
            }
        }
        #endregion
    }
}
