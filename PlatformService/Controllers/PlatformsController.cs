using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo repo;
        private readonly IMapper mapper;
		private readonly ICommandDataClient commandDataClient;
        private readonly IMessageClientBus messageClientBus;

        public PlatformsController(IPlatformRepo repo, IMapper mapper, 
            ICommandDataClient commandDataClient, IMessageClientBus messageClientBus)
        {
            this.repo = repo;
            this.mapper = mapper;
			this.commandDataClient = commandDataClient;
            this.messageClientBus = messageClientBus;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var platforms = repo.GetAllPlatforms();
            var platformsDto = mapper.Map<IEnumerable<PlatformReadDto>>(platforms);
            return Ok(platformsDto);
        }

        [HttpGet("{id}", Name ="GetPlatformById")]
        public IActionResult GetPlatformById(int id)
        {
            var platform = repo.GetPlatformById(id);
            if(platform != null)
            {
                return Ok(mapper.Map<PlatformReadDto>(platform));
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PlatformCreateDto platformDto )
        {
            var platform = mapper.Map<Platform>(platformDto);
            repo.CreatePlatform(platform);
            repo.SaveChanges();

            var platformReadDto = mapper.Map<PlatformReadDto>(platform);

            //send synchronously paltform to commandservice
            try
            {
                await commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"---> Could not send synchronously: {ex.Message}");
            }

            //send asynchronously platform to message bus
            try
            {
                var platformPublishedDto = mapper.Map<PlatformPublishedDto>(platformReadDto);
                platformPublishedDto.Event = "Platform Published";
                messageClientBus.PublishNewPlatform(platformPublishedDto);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"---> Could not send asynchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id}, platformReadDto);
        }
    }
}
