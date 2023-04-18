using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CommandService.Controllers
{
    [Route("api/commands/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandServiceRepo repo;
        private readonly IMapper mapper;

        public CommandsController(ICommandServiceRepo repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            if (!repo.PlatformExists(platformId))
            {
                return NotFound("Platform is not found");
            }

            var commands = repo.GetCommandsForPlatform(platformId);
            var commandsDto = mapper.Map<IEnumerable<CommandReadDto>>(commands);
            return Ok(commandsDto);
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
        {
            if (!repo.PlatformExists(platformId))
            {
                return NotFound("Platform is not found");
            }

            var command = repo.GetCommand(commandId, platformId);
            var commandDto = mapper.Map<CommandReadDto>(command);

            if (command == null)
            {
                return NotFound("Command is not found");
            }

            return Ok(commandDto);
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandDto)
        {
            if (!repo.PlatformExists(platformId))
            {
                return NotFound("Platform is not found");
            }
            var command = mapper.Map<Command>(commandDto);
            repo.CreateCommand(platformId, command);
            repo.SaveChanges();

            var commandReadDto = mapper.Map<CommandReadDto>(command);
            return CreatedAtRoute(nameof(GetCommandForPlatform), new {commandId = commandReadDto.Id, platformId = platformId} ,commandReadDto);
        }
    }
}
