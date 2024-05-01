
using API_TODO.Models;
using API_TODO.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API_TODO
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			//add db postgreSQL
			builder.Services.AddDbContext<DbtodoContext>(opt =>
			{
				opt.UseNpgsql(builder.Configuration.GetConnectionString("PosgreSQLConnection"));
			});

			//add scoped autorizacion
			builder.Services.AddScoped<IAutorizacionService, AutorizacionService>();


			//get key
			var key = builder.Configuration.GetValue<string>("JwtSettings:key");
			var keyBytes = Encoding.ASCII.GetBytes(key);


			//add Authenticacion
			builder.Services.AddAuthentication(config =>
			{
				config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(config =>
			{
				config.RequireHttpsMetadata = false;
				config.SaveToken = true;
				config.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero
				};
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
