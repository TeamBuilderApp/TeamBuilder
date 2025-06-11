using Microsoft.AspNetCore.Mvc;
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
                .Select(x => TeamBuilder.TeamBuilderEventToDto(x))
                .ToList(); 
        }*/

        //GETTER
        // GET: TeamBuilder
        //HTTP GET(s) all Team Builder Event(s).
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamBuilderEventDto>>> GetTeamBuilder()
        {
            return await _context.TeamBuilder
                .Select(x => TeamBuilder.Models.TeamBuilder.ObjectToDto(x))
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
            TeamBuilder.Models.TeamBuilder? teamBuilderEvent = await _context.TeamBuilder.FindAsync(id);

            if (teamBuilderEvent == null)
            {
                return NotFound(); //404
            }

            return TeamBuilder.Models.TeamBuilder.ObjectToDto(teamBuilderEvent);
        }
        // </snippet_Get>

        // <snippet_Update>
        //UPDATE
        //The PUT method is used to update a single resource by its ID.
        // Exact match. Updates a Team Builder Event via lookup by its ID.
        // PUT: TeamBuilder/id
        // Example: TeamBuilder/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeamBuilderEvent(long id, TeamBuilderEventDto teamBuilderEventDto)
        {
            teamBuilderEventDto.Id = id;

            var teamBuilderEvent = await _context.TeamBuilder.FindAsync(id);
            if (teamBuilderEvent == null)
            {
                return NotFound(); //404
            }

            _ = Util.Util.CopyProperties(teamBuilderEventDto, teamBuilderEvent);

            try
            {
                _ = await _context.SaveChangesAsync();
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
        public async Task<ActionResult<TeamBuilderEventDto>> PostTeamBuilderEvent(TeamBuilderEventDto teamBuilderEventDto)
        {
            Models.TeamBuilder teamBuilderEvent = new(teamBuilderEventDto);

            try
            {
                //Todo test delete me:
                //Util.Util.readFiles();


                //Todo re-enable Azure sub. Save to DB.
                _ = _context.TeamBuilder.Add(teamBuilderEvent);
                _ = await _context.SaveChangesAsync();
                teamBuilderEventDto.Id = teamBuilderEvent.Id;
            }
            catch (Exception)
            {
                return NotFound(); //404
            }

            return CreatedAtAction(
                nameof(GetTeamBuilderEvent),
                new { id = teamBuilderEvent.Id },
                teamBuilderEventDto);
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
            TeamBuilder.Models.TeamBuilder? teamBuilderEvent = await _context.TeamBuilder.FindAsync(id);
            if (teamBuilderEvent == null)
            {
                return NotFound(); //404
            }

            try
            {
                _ = _context.TeamBuilder.Remove(teamBuilderEvent);
                _ = await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TeamBuilderEventExists(id))
            {
                return NotFound(); //404
            }

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
