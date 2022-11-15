using Hangfire;
using Hangfire.Console;
using Hangfire.Console.Extensions;
using Hangfire.MemoryStorage;
using Microsoft.EntityFrameworkCore;
using HtmlConverter.Persistence.Context;
using HtmlConverter.Application.Interfaces;
using HtmlConverter.Persistence.Repositories;
using HtmlConverter.Application.FileConverter;

namespace HtmlConverter.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<FilesConverterDbContext>(opt => opt.UseInMemoryDatabase("converter"));
            services.AddTransient<IBaseRepository<Domain.Models.File>, FilesRepository>();

            services.AddTransient<IConverterJobStatus, ConverterJobStatus>();
            services.AddTransient<IFileStore, FileStore>();
            services.AddTransient<IFileConverter, FileConverter>();
            
            services.AddHangfire(config =>
            {
                config.UseMemoryStorage();
                config.UseConsole();
            });

            services.AddHangfireConsoleExtensions();
            services.AddHangfireServer();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseHangfireDashboard();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
