using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

		public PlatformsController(IPlatformRepo repo, IMapper mapper, ICommandDataClient commandDataClient)
        {
            this.repo = repo;
            this.mapper = mapper;
			this.commandDataClient = commandDataClient;
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

            //send paltform to commandservice
            try
            {
                await commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"---> Could not send synchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id}, platformReadDto);
        }
    }
}
