using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Task_Management_System.Models;
using Task_Management_System.taskManagement.Context;

namespace Task_Management_System.Controllers
{
    [ApiController]
    [Route("api/Tasks")]
    [Authorize]
    public class TasksApiController : ControllerBase
    {
        private readonly TaskManagementDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TasksApiController(TaskManagementDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetAllTasks()
        {
            var tasks = await _context.Task.ToListAsync();


            return tasks;
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Task>> GetTaskById(int id)
        {
            var task = await _context.Task.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        // POST: api/Tasks
        [HttpPost]
        public async Task<ActionResult<Models.Task>> CreateTask(Models.Task task)
        {
            _context.Task.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }

        // PUT: api/Tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, Models.Task task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Task.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Task.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskExists(int id)
        {
            return _context.Task.Any(e => e.Id == id);
        }

        // POST: api/Tasks/Assign/{taskId}/{userId}
        [HttpPost("Assign/{taskId}/{userName}")]
        public async Task<IActionResult> AssignTask(int taskId, string userName)
        {
            var task = await _context.Task.FindAsync(taskId);
            var user = await _userManager.FindByNameAsync(userName);

            if (task == null || user == null)
            {
                return NotFound();
            }

            task.AssignedUserId = user.Id;
            _context.Update(task);
            await _context.SaveChangesAsync();

            return Ok("Task assigned successfully.");
        }

        // GET: api/Tasks/Search?keywords={keywords}
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<Models.Task>>> SearchTasks(string keywords)
        {
            var tasks = _context.Task
                .Where(task => task.Title.Contains(keywords) || task.Description.Contains(keywords))
                .ToList();

            return tasks;
        }

        // GET: api/Tasks/SortByPriority
        [HttpGet("SortByPriority")]
        public async Task<ActionResult<IEnumerable<Models.Task>>> SortTasksByPriority()
        {
            var tasks = _context.Task
                .OrderBy(task => task.Priority)
                .ToList();

            return tasks;
        }

        // GET: api/Tasks/SortByDueDate
        [HttpGet("SortByDueDate")]
        public async Task<ActionResult<IEnumerable<Models.Task>>> SortTasksByDueDate()
        {
            var tasks = _context.Task
                .OrderBy(task => task.DueDate)
                .ToList();

            return tasks;
        }

        // POST: api/Tasks/Assign/{taskId}/{userId}
        [HttpPost("AddComment/{taskId}/{userName}")]
        public async Task<IActionResult> AddComment(int taskId, string userName,string content)
        {
            var task = await _context.Task.FindAsync(taskId);
            var user = await _userManager.FindByNameAsync(userName);

            if (task == null || user == null || content==null)
            {
                return NotFound();
            }

            var comment = new Comment
            {
                UserId = user.Id,
                TaskId = task.Id,
                Content = content
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return Ok("Comment assigned successfully.");
        }


    }
}
