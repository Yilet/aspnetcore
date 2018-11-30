
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserAPI2.Data
{
    public class UserContextSeed
    {
        private ILogger<UserContextSeed> _logger;

        public UserContextSeed(ILogger<UserContextSeed> logger)
        {
            _logger = logger;
        }

        public static async Task SeedAsync(IApplicationBuilder applicationBuilder, ILoggerFactory loggerFactory,int? retry=0)
        {
            var retryForAvaiability = retry.Value;
            try
            {
                using (var scope = applicationBuilder.ApplicationServices.CreateScope())
                {
                    var context = (ApiUserContext)scope.ServiceProvider.GetService(typeof(ApiUserContext));
                    var logger = (ILogger<UserContextSeed>)scope.ServiceProvider.GetService(typeof(ILogger<UserContextSeed>));
                    logger.LogDebug("Begin UserContextSeed SeedAsync");

                    context.Database.Migrate();

                    var anyUser = context.Users;
                    if (anyUser.Count() <= 0)
                    {
                        context.Users.Add(new Models.AppUser { Name = "guandex" });
                        context.SaveChanges();
                    }


                    //if (!context.Users.Any())
                    //{
                    //    context.Users.Add(new Models.AppUser { Name = "guandex" });
                    //    context.SaveChanges();
                    //}
                }
            }
            catch (Exception ex)
            {
                if (retryForAvaiability < 10)
                {
                    retryForAvaiability++;

                    var logger = loggerFactory.CreateLogger(typeof(UserContextSeed));
                    logger.LogError(ex.Message);

                    await SeedAsync(applicationBuilder, loggerFactory, retryForAvaiability);
                }
            }
        }
    }
}
