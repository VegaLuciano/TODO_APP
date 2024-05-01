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
		[Route("List")]
		public async Task<IActionResult> List(int idUser)
		{
			var list = _context.Tasks.OrderByDescending(t => t.Date).ToList();

			return Ok(list);
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

				return Ok();
			}
			catch (DbUpdateException ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error at add task: " + ex.Message);
			}
		}

		[HttpDelete]
		[Route("DeleteTask")]
		public async Task<IActionResult> DeleteTask(string idTask)
		{
			// Validar la entrada idTask
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

	}
}
