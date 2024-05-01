using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API_TODO.Models;
using API_TODO.Models.Custom;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Security.Cryptography;

namespace API_TODO.Service
{
	public class AutorizacionService : IAutorizacionService
	{
		private readonly DbtodoContext _context;
        private readonly IConfiguration _configuration;

        public AutorizacionService(DbtodoContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

		private string GenerateToken(string idUser)
		{

			var key = _configuration.GetValue<string>("JwtSettings:key");
			var keyBytes = Encoding.ASCII.GetBytes(key);

			var claims = new ClaimsIdentity();
			claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, idUser));

			var credencialesToken = new SigningCredentials(
				new SymmetricSecurityKey(keyBytes),
				SecurityAlgorithms.HmacSha256Signature
				);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = claims,
				Expires = DateTime.UtcNow.AddMinutes(1),
				SigningCredentials = credencialesToken
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

			string tokenCreado = tokenHandler.WriteToken(tokenConfig);

			return tokenCreado;


		}
		public async Task<AutorizacionResponse> ReturnToken(AutorizacionRequest autorizacion)
		{
			var usuario_encontrado = _context.Users.FirstOrDefault(x =>
				x.Email == autorizacion.Email &&
				x.Password == autorizacion.Password
			);

			if (usuario_encontrado == null)
			{
				return await System.Threading.Tasks.Task.FromResult<AutorizacionResponse>(null);
			}

			string tokenCreado = GenerateToken(usuario_encontrado.Id.ToString());

			return new AutorizacionResponse() { Token = tokenCreado, Resultado = true, Msg = "Ok" };

		}

	}
}
