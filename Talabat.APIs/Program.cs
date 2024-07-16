
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories.Contract;
using Talabat.Infrastructure;
using Talabat.Infrastructure.Data;
using Talabat.Infrastructure.Identity;
using Talabat.Infrastructure.Identity.DataSeed;

namespace Talabat.APIs
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var webApplicationBuilder = WebApplication.CreateBuilder(args);

			#region Configure Services

			webApplicationBuilder.Services.ApplicationServices(webApplicationBuilder.Configuration);

			#endregion

			var app = webApplicationBuilder.Build();

			using var scope = app.Services.CreateScope();

			var services = scope.ServiceProvider;

			var _dbContext = services.GetRequiredService<StoreContext>();
			var _identityDbContext = services.GetRequiredService<ApplicationIdentityDbContext>();
			var _userManger = services.GetRequiredService<UserManager<ApplicationUser>>();
			var _loggerFactory = services.GetRequiredService<ILoggerFactory>();

			try
			{
				await _dbContext.Database.MigrateAsync();
				await StoreContextSeed.SeedAsync(_dbContext);

				await _identityDbContext.Database.MigrateAsync();
				await ApplicationIdentityDataSeed.SeedUserAsync(_userManger);
			}
			catch (Exception ex)
			{

				var loger = _loggerFactory.CreateLogger<Program>();
				loger.LogError(ex, "an Error Has Been Occured during apply the migration");

			}


			#region Configure Kestrel Middlewares

			app.UseMiddleware<ExceptionMiddleware>();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			
			app.UseStatusCodePagesWithReExecute("/errors/{0}");

			app.UseHttpsRedirection();

			app.UseCors("MyPolicy");

			app.MapControllers();

			app.UseStaticFiles();


			#endregion

			app.Run();

		}
	}
}
