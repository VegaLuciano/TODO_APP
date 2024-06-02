using API_TODO.Models;
using API_TODO.Models.Custom;
using API_TODO.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace API_TODO.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class TaskController : ControllerBase
	{
		private readonly DbtodoContext _context;

		public TaskController(DbtodoContext context)
		{
			_context = context;
		}


		[HttpGet]
		[Route("ListTask")]
		public async Task<IActionResult> List(int idUser)
		{
			if (idUser == null || idUser == 0 )
			{
				// Si no se proporciona ningún valor para idUser, devolver un BadRequest
				return BadRequest("User ID is required.");
			}
			var list = await _context.Tasks.Where(t => t.Userid == idUser).OrderBy(t => t.Dateregister).ToListAsync();

			if (list.Any())		
				return Ok(list);		
			else if (!await _context.Users.AnyAsync(u => u.Id == idUser))		
				return NotFound("User not found.");
			else		
				return NotFound("No tasks found for this user."); ;
			
		}

		[HttpPost]
		[Route("AddTask")]
		public async Task<IActionResult> AddTask([FromBody] Models.Task task)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			try
			{
				await _context.Tasks.AddAsync(task);
				await _context.SaveChangesAsync();

				return StatusCode(StatusCodes.Status200OK, "Task added");
			}
			catch (DbUpdateException ex)
			{
				return StatusCode(500, "Error at add task: " + ex.Message);
			}
		}

		[HttpDelete]
		[Route("DeleteTask")]
		public async Task<IActionResult> DeleteTask(string idTask)
		{
	
			if (!Guid.TryParse(idTask, out Guid taskId))
			{
				return BadRequest("Invalid task ID format");
			}

			try
			{
				var task = await _context.Tasks.FindAsync(taskId);

				if (task == null)
				{
					return NotFound("Task not found");
				}

				_context.Tasks.Remove(task);
				await _context.SaveChangesAsync();

				// Registrar la acción en los logs
				//_logger.LogInformation($"Task with ID {idTask} deleted successfully.");

				return Ok("Task deleted");
			}
			catch (Exception ex)
			{
				//_logger.LogError($"Error deleting task with ID {idTask}: {ex.Message}");
				return StatusCode(500, "An error occurred while deleting the task");
			}
		}
		[HttpPut]
		[Route("UpdateTask")]
		public async Task<IActionResult> UpdateTask([FromBody] Models.Task task)
		{

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				_context.Tasks.Update(task);
				await _context.SaveChangesAsync();

				// Registrar la acción en los logs
				return Ok("Task updated");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"An error occurred while updating the task {ex.Message}");
			}
		}
	}
}
