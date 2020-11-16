using DakarRally;
using Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

public class TestingWebAppFactory<T> : WebApplicationFactory<Startup>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {

            var descriptor = services.SingleOrDefault(
              d => d.ServiceType ==
                 typeof(DbContextOptions<RepositoryContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            var inMemorySqlite = new SqliteConnection("Data Source=:memory:");
            inMemorySqlite.Open();
            services.AddDbContext<RepositoryContext>(o => o.UseSqlite(inMemorySqlite));

            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<RepositoryContext>())
                {
                    try
                    {
                        appContext.Database.EnsureCreated();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
        });
    }
}