﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamBuilder.Models;

namespace TeamBuilder.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamBuilderController : ControllerBase
    {
        private readonly TeamBuilderContext _context;

        private readonly ILogger<TeamBuilderController> _logger;

        //Example: Converts a value by ID match, when creating a new object.
        /*private static readonly string[] SetNameIdMapping = new[]
        {
            "a", "b", "c", "d"
        };*/

        //Example of how to use logging.
        //public TeamBuilderController(ILogger<TeamBuilderController> logger)
        public TeamBuilderController(TeamBuilderContext context, ILogger<TeamBuilderController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // <snippet_Get>
        //The most basic HTTP Get example.
        /*
        [HttpGet(Name = "GetTeamBuilder")]
        public IEnumerable<TeamBuilderEventDto> Get()
        {
            return _context.TeamBuilder
                .Select(x => TeamBuilderEvent.TeamBuilderEventToDto(x))
                .ToList(); 
        }*/

        //GETTER
        // GET: TeamBuilder
        //HTTP GET(s) all Team Builder Event(s).
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamBuilderEventDto>>> GetTeamBuilder()
        {
            return await _context.TeamBuilder
                .Select(x => TeamBuilderEvent.TeamBuilderEventToDto(x))
                .ToListAsync();
        }

        //GETTER
        //GET: TeamBuilder/id
        //Example: TeamBuilder/5
        //Getter by id.
        // Exact match. HTTP GETs a Team Builder Event via lookup by its ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamBuilderEventDto>> GetTeamBuilderEvent(long id)
        {
            var teamBuilderEvent = await _context.TeamBuilder.FindAsync(id);

            if (teamBuilderEvent == null)
            {
                return NotFound(); //404
            }

            return TeamBuilderEvent.TeamBuilderEventToDto(teamBuilderEvent);
        }
        // </snippet_Get>

        // <snippet_Update>
        //SETTER
        //The PUT method is used to update a single resource by its ID.
        // Exact match. Updates a Team Builder Event via lookup by its ID.
        // PUT: TeamBuilder/id
        // Example: TeamBuilder/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeamBuilderEvent(long id, TeamBuilderEventDto TeamBuilderEventDto)
        {
            if (id != TeamBuilderEventDto.Id)
            {
                return BadRequest(); //400
            }

            var teamBuilderEvent = await _context.TeamBuilder.FindAsync(id);
            if (teamBuilderEvent == null)
            {
                return NotFound(); //404
            }

            Util.Util.CopyProperties(TeamBuilderEventDto, teamBuilderEvent);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TeamBuilderEventExists(id))
            {
                return NotFound(); //404
            }

            return NoContent();
        }
        // </snippet_Update>

        // <snippet_Create>
        // CREATE
        // The POST method is used to create a new resource.
        // POST: TeamBuilder
        // Example: TeamBuilder
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TeamBuilderEventDto>> PostTeamBuilderEvent(TeamBuilderEventDto TeamBuilderEventDto)
        {
            var teamBuilderEvent = new TeamBuilderEvent(TeamBuilderEventDto);


            _context.TeamBuilder.Add(teamBuilderEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetTeamBuilderEvent),
                new { id = teamBuilderEvent.Id },
                TeamBuilderEvent.TeamBuilderEventToDto(teamBuilderEvent));
        }
        // </snippet_Create>


        // <snippet_Delete>
        // DELETE
        // The DELETE method is used to delete an existing resource by its ID.
        // Exact match. Deletes a Team Builder Event via lookup by its ID.
        // DELETE: TeamBuilder/id
        // Example: TeamBuilder/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeamBuilderEvent(long id)
        {
            var teamBuilderEvent = await _context.TeamBuilder.FindAsync(id);
            if (teamBuilderEvent == null)
            {
                return NotFound(); //404
            }

            _context.TeamBuilder.Remove(teamBuilderEvent);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // </snippet_Delete>

        // Exact match. Finds a Team Builder Event via lookup by its ID.
        private bool TeamBuilderEventExists(long id)
        {
            return _context.TeamBuilder.Any(e => e.Id == id);
        }
    }
}
