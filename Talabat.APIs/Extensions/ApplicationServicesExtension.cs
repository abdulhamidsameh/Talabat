using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Helpers;
using Talabat.Core.Repositories.Contract;
using Talabat.Infrastructure.Data;
using Talabat.Infrastructure;
using Talabat.APIs.Errors;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.Infrastructure.Basket_Repository;
using Talabat.Infrastructure.Generic_Repository;
using Talabat.Infrastructure.Identity;
using Talabat.Core.Entities.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Talabat.Core.Services.Contract;
using Talabat.Application.AuthService;

namespace Talabat.APIs.Extensions
{
	public static class ApplicationServicesExtension
	{
		public static IServiceCollection ApplicationServices(this IServiceCollection services, IConfiguration configuration)
		{
			// Add services to the container.

			services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();
			services.AddAutoMapper(typeof(MappingProfile));
			services.AddDbContext<StoreContext>(options =>
			{
				options.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]).UseLazyLoadingProxies();
			});

			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = (actionContext) =>
				{
					var errors = actionContext.ModelState.Where(P => P.Value?.Errors.Count > 0)
					.SelectMany(P => P.Value?.Errors!)
					.Select(E => E.ErrorMessage)
					.ToList();

					var response = new ApiValidationErrorsResponse()
					{
						Errors = errors
					};

					return new BadRequestObjectResult(response);
				};
			});

			services.AddSingleton<IConnectionMultiplexer>(serviceProvider =>
			{
				var connection = configuration.GetConnectionString("Redis");
				return ConnectionMultiplexer.Connect(connection!);
			});

			services.AddScoped<IBasketRepository, BasketRepository>();

			services.AddDbContext<ApplicationIdentityDbContext>(options =>
			{
				options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));

			});

			services.AddIdentity<ApplicationUser,IdentityRole>()
				.AddEntityFrameworkStores<ApplicationIdentityDbContext>();

			services.AddScoped<IAuthService, AuthService>();

			return services;
		}
	}
}
