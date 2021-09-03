using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repo;
        private readonly IMapper _mapper;
        private readonly ICommandDataClients _commandDataClient;

        public PlatformsController(
            IPlatformRepo repo, 
            IMapper mapper, 
            ICommandDataClients commandDataClient)
        {
            _repo = repo;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
        }
        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDTO>> GetAllPlatform()
        {
            var platforms = _repo.GetAllPlatforms();
            var platformsDTO = _mapper.Map<IEnumerable<PlatformReadDTO>>(platforms);
            return Ok(platformsDTO);
        }
        [HttpGet("{id:int}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDTO>GetPlatformById(int id)
        {
            var platform = _repo.GetPlatformById(id);
            if(platform  != null)
            {
                return Ok(_mapper.Map<PlatformReadDTO>(platform));
            }
            return NotFound();
        }
        [HttpPost]
         public async Task<ActionResult<PlatformReadDTO>> CreatePlatform(PlatformCreateDTO platformCreateDto)
        {
            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            _repo.CreatePlatform(platformModel);
            _repo.SaveChanges();

            var platformReadDto = _mapper.Map<PlatformReadDTO>(platformModel);

            // Send Sync Message
            try
            {
                await _commandDataClient.SendNewPlatformToCommand(platformReadDto);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id}, platformReadDto);
        }
    }
}