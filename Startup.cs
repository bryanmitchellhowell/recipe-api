using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipeApi.Helpers;
using RecipeApi.Services;

namespace RecipeApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // add services to the DI container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddScoped<IRecipeService, RecipeService>();
            services.AddScoped<IUserService, UserService>();

            services.AddCors(options => {
                options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RecipeApi", Version = "v1" });
            //});


            // configure strongly typed settings object
            //services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            //// configure DI for application services
            //services.AddScoped<IUserService, UserService>();

            //services.AddCors();
            //services.AddControllers();
        }

        // configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            // custom jwt auth middleware
            //app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(x => x.MapControllers());
        }
    }
}
