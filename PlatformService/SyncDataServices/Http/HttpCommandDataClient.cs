using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PlatformService.DTOs;

namespace PlatformService.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClients
    {
        private readonly ICommandDataClients _commandDataClient;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public HttpCommandDataClient(ICommandDataClients commandDataClient, HttpClient httpClient, IConfiguration config)
        {
            _commandDataClient = commandDataClient;
            _httpClient = httpClient;
            _config = config;
        }
        public async Task SendNewPlatformToCommand(PlatformReadDTO platformDTO)
        {
            
            var content = new StringContent(
                JsonSerializer.Serialize(platformDTO),
               Encoding.UTF8,
               "application/json");
            
            var response = await _httpClient.PostAsync($"{_config["CommandConn"]}",content);

            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to CommandService was OK!");
            }
            else
            {
                Console.WriteLine("--> Sync POST to CommandService was NOT OK!");
            }
        }
    }
}