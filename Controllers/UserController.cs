using API_TODO.Models;
using API_TODO.Models.Custom;
using API_TODO.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace API_TODO.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IAutorizacionService _autorizacionService;
		private readonly DbtodoContext _context;

		public UserController(IAutorizacionService autorizacionService, DbtodoContext context)
		{
			_autorizacionService = autorizacionService;
			_context = context;
		}

		[HttpPost]
		[Route("authenticate")]
		public async Task<IActionResult> Autenticar([FromBody] AutorizacionRequest autorizacion)
		{
			var resultado_autorizacion = await _autorizacionService.ReturnToken(autorizacion);
			if (resultado_autorizacion == null)
				return Unauthorized();

			return Ok(resultado_autorizacion);

		}

		[HttpPost]
		[Route("AddUser")]
		public async Task<IActionResult> AddUser([FromBody] User user)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				await _context.Users.AddAsync(user);
				await _context.SaveChangesAsync();

				return Ok();
			}
			catch (DbUpdateException ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error at add user: " + ex.Message);
			}
		}

		[HttpGet]
		[Route("SelectUser")]
		public async Task<IActionResult> SelectUser(int idUser)
		{
			var user = _context.Users.FirstOrDefault(x => x.Id == idUser);

			if (user == null)
			{
				return NotFound("Usuario not found");
			}

			return Ok(user);
		}

		[HttpDelete]
		[Route("DeleteUser")]
		public async Task<IActionResult> DeleteUser(int idUser)
		{
			var user = _context.Users.FirstOrDefault(x => x.Id == idUser);

			if (user == null)
			{
				return NotFound("Usuario not found");
			}

			_context.Users.Remove(user);
			await _context.SaveChangesAsync();

			return Ok("User deleted");
		}

	}
}
