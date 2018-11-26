using System.Text;
using CitiesWeb.API.Controllers;
using CitiesWeb.API.Data;
using CitiesWeb.API.Helpers;
using CitiesWeb.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NLog.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;

namespace CitiesWeb.API
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CitiesDbContext>(options => options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));


            services.AddScoped<ICitiesRepository, CitiesRepository>();


            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "Cities API", Version = "v1" });
            });

            services.AddCors();
            
            var appSettingsSections = _configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSections);

            var appSettings = appSettingsSections.Get<AppSettings>();

            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddScoped<IUserService, UserService>();

            services.AddMvc(options => 
            {
                options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(ILoggerFactory loggerFactory, IApplicationBuilder app, IHostingEnvironment env, CitiesDbContext context)
        {
            loggerFactory.AddNLog();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            context.EnsureSeedDataForContext();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = string.Empty;
                });
            }

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.City, Models.CityWithoutPointsOfInterestDto>().ReverseMap();
                cfg.CreateMap<Entities.City, Models.CityDTO>().ReverseMap();
                cfg.CreateMap<Entities.PointOfInterest, Models.PointOfInterestDTO>().ReverseMap();
                cfg.CreateMap<Models.PointOfInterestForCreationDto, Entities.PointOfInterest>().ReverseMap();
                cfg.CreateMap<Models.PointOfInterestForUpdateDto, Entities.PointOfInterest>().ReverseMap();
                cfg.CreateMap<Entities.PointOfInterest, Models.PointOfInterestForUpdateDto>().ReverseMap();
            });

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
