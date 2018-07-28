﻿using System;
using System.Collections.Generic;
using System.IO;
using ExampleDatabase;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExampleWebApi.Helpers
{
    public static class DatabaseStartupHelpers
    {

        public static IWebHost SetupDevelopmentDatabase(this IWebHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                using (var context = services.GetRequiredService<ExampleDbContext>())
                {
                    try
                    {
                        context.Database.EnsureCreated();
                        context.SeedDatabase();
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred while setting up or seeding the development database.");
                    }
                }
            }

            return webHost;
        }

        private static void SeedDatabase(this ExampleDbContext context)
        {
            var startupItems = new List<TodoItem>
            {
                new TodoItem("Create ASP.NET Core API project", 1),
                new TodoItem("Create simple EF Core database", 1),
                new TodoItem("Add EfCore.GenericServices to web app", 1),
                new TodoItem("Create a example WebAPI controller", 3),
                new TodoItem("Write unit tests", 2),
                new TodoItem("Add Swagger for manual testing", 2)
            };
            context.AddRange(startupItems);
            context.SaveChanges();
        }
    }
}