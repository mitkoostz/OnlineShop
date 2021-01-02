
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ifrastructure.Data;
using AutoMapper;
using Api.Helpers;
using Api.Middleware;
using Api.Extensions;
using StackExchange.Redis;
using Infrastructure.Identity;
using Microsoft.AspNetCore.HttpOverrides;

namespace Api
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration configuration)
        {
               _config = configuration;
               
        }

       // public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddControllers();

            services.AddDbContext<StoreContext>(options =>
            options.UseSqlServer(_config.GetConnectionString("DefaultConnection")));
            services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseSqlServer(_config.GetConnectionString("IdentityConnection")));
            // services.AddDbContext<StoreContext>(x => 
            // x.UseSqlite(_config.GetConnectionString("DefaultConnection")));
            // services.AddDbContext<AppIdentityDbContext>( x => {
                 
            //     x.UseSqlite(_config.GetConnectionString("IdentityConnection"));
            // });

            services.AddSingleton<IConnectionMultiplexer>(c => {
                var configuration = ConfigurationOptions.Parse(_config
                .GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(configuration);

            });

            services.AddApplicationServices();
            services.AddIdentityServices(_config);
            services.AddSwaggerDocumentation();
            services.AddCors( opt => 
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseStaticFiles();
            app.UseCors("CorsPolicy");

            //app.UseForwardedHeaders(new ForwardedHeadersOptions
            //{
            //    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
            //    ForwardedHeaders.XForwardedProto
            //});

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwaggerDocumentation();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
