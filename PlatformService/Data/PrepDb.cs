using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepareData(IApplicationBuilder builder)
        {
            using(var scope = builder.ApplicationServices.CreateScope())
            {
                SeedingData(scope.ServiceProvider.GetService<AppDbContext>());
            }
        }
        private static void SeedingData(AppDbContext context)
        {
            if(!context.Platforms.Any())
            {
                Console.WriteLine("Seeding Data....");
                context.Platforms.AddRange(
                    new Platform() {Name="Dot Net", Publisher="Microsoft", Cost="Free"},
                    new Platform() {Name="SQL Server Express", Publisher="Microsoft",  Cost="Free"},
                    new Platform() {Name="Kubernetes", Publisher="Cloud Native Computing Foundation",  Cost="Free"}
                );
                context.SaveChanges();
            }
            else 
            {
                Console.WriteLine("We have data already");
            }

        }
    }
}