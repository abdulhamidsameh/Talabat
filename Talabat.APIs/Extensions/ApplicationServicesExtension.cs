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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.Core.UnitOfWork.Contract;
using Talabat.Infrastructure._UnitOfWork;
using Talabat.Application.OrderService;

namespace Talabat.APIs.Extensions
{
	public static class ApplicationServicesExtension
	{
		public static IServiceCollection ApplicationServices(this IServiceCollection services, IConfiguration configuration)
		{
			// Add services to the container.

			services.AddControllers()
				.AddNewtonsoftJson(options =>
				{
					options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
				});

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();
			services.AddAutoMapper(typeof(MappingProfile));
			services.AddDbContext<StoreContext>(options =>
			{
				options.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]).UseLazyLoadingProxies();
			});

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
				options.UseSqlServer(configuration.GetConnectionString("IdentityConnection")).UseLazyLoadingProxies();

			});

			services.AddIdentity<ApplicationUser,IdentityRole>()
				.AddEntityFrameworkStores<ApplicationIdentityDbContext>();

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters()
					{
						ValidateIssuer = true,
						ValidIssuer = configuration["JWT:ValidIssuer"],
						ValidateAudience = true,
						ValidAudience = configuration["JWT:ValidAudience"],
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:AuthKey"] ?? string.Empty)),
						ValidateLifetime = true,
						ClockSkew = TimeSpan.Zero,
					
					};
				});

			services.AddScoped<IAuthService, AuthService>();

			services.AddScoped<IUnitOfWork, UnitOfWork>();

			services.AddScoped<IOrderService, OrderService>();

			return services;
		}
	}
}
