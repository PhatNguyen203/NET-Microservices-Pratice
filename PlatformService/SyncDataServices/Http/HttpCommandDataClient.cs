using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlatformService.SyncDataServices.Http
{
	public class HttpCommandDataClient : ICommandDataClient
	{
		private readonly HttpClient client;
		private readonly IConfiguration config;

		public HttpCommandDataClient(HttpClient client, IConfiguration config )
		{
			this.client = client;
			this.config = config;
		}

		public async Task SendPlatformToCommand(PlatformReadDto platform)
		{
			var httpContent = new StringContent(
				JsonSerializer.Serialize(platform),
				Encoding.UTF8,
				"application/json"
				);
			var response = await client.PostAsync($"{config["CommandService"]}/api/commands/platforms", httpContent);

			if (response.IsSuccessStatusCode)
			{
				Console.WriteLine("---> Sync POST to CommandService was OK!");
			}
			else
			{
				Console.WriteLine($"---> Sync POST to CommandService has some issues! {response.ReasonPhrase}");
			}
		}
	}
}
