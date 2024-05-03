using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamBuilder.Models;

namespace TeamBuilder.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        private readonly ILogger<TodoItemsController> _logger;

        //Example: Converts a value by ID match, when creating a new object.
        /*private static readonly string[] SetNameIdMapping = new[]
        {
            "a", "b", "c", "d"
        };*/

        //Example of how to use logging.
        //public TodoItemsController(ILogger<TodoItemsController> logger)
        public TodoItemsController(TodoContext context, ILogger<TodoItemsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // <snippet_Get>
        //The most basic HTTP Get example.
        /*
        [HttpGet(Name = "GetTodoItems")]
        public IEnumerable<TodoItemDTO> Get()
        {
            return _context.TodoItems
                .Select(x => TodoItem.ItemToDTO(x))
                .ToList(); 
        }*/

        //GETTER
        // GET: TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            return await _context.TodoItems
                .Select(x => TodoItem.ItemToDTO(x))
                .ToListAsync();
        }

        //GETTER
        //GET: TodoItems/id
        //Example: TodoItems/5
        //Getter by id.
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return TodoItem.ItemToDTO(todoItem);
        }
        // </snippet_Get>

        // <snippet_Update>
        //SETTER
        //The PUT method is used to update a single resource.
        // PUT: TodoItems/id
        // Example: TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoDTO)
        {
            if (id != todoDTO.Id)
            {
                return BadRequest();
            }

            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Name = todoDTO.Name;
            todoItem.IsComplete = todoDTO.IsComplete;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }
        // </snippet_Update>

        // <snippet_Create>
        // CREATE
        // The POST method is used to create a new resource.
        // POST: TodoItems
        // Example: TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO todoDTO)
        {
            var todoItem = new TodoItem
            {
                IsComplete = todoDTO.IsComplete,
                Name = todoDTO.Name
            };

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetTodoItem),
                new { id = todoItem.Id },
                TodoItem.ItemToDTO(todoItem));
        }
        // </snippet_Create>


        // <snippet_Delete>
        // DELETE
        // The DELETE method is used to delete an existing resource.
        // DELETE: TodoItems/id
        // Example: TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // </snippet_Delete>

        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }
    }
}
