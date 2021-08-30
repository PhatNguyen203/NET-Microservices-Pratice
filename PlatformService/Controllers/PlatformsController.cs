using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.Models;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repo;
        private readonly IMapper _mapper;

        public PlatformsController(IPlatformRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
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
        public ActionResult CreateNewPlatform(PlatformCreateDTO newPlatformDTO)
        {
            var platform = _mapper.Map<Platform>(newPlatformDTO);
            _repo.CreatePlatform(platform);
            _repo.SaveChanges();
            var platformDTO = _mapper.Map<PlatformReadDTO>(platform);
            return CreatedAtRoute(nameof(GetPlatformById), new {Id = platformDTO.Id}, platformDTO );
        }
    }
}