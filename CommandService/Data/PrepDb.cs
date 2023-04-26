using CommandService.Models;
using CommandService.SyncDataServices.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandService.Data
{
    public static class PrepDb
    {
        public static void PopulationData(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                
                var grpcClient = scope.ServiceProvider.GetServices<IPlatformDataClient>().First();
                var platforms = grpcClient.ReturnAllPlatforms();
                //seeding platforms data
                SeedData(scope.ServiceProvider.GetService<ICommandServiceRepo>(), platforms);
            }
        }
        private static void SeedData(ICommandServiceRepo repo, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("Seeding new platforms...");
            foreach(var platform in platforms)
            {
                if(!repo.ExternalPlatformExists(platform.ExternalId))
                {
                    repo.CreatPlatform(platform);
                }
            }
            repo.SaveChanges();

        }
    }
}
