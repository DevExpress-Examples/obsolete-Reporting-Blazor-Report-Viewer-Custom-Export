using BlazorExportCustomization.CustomExport;
using BlazorExportCustomization.Data;
using DevExpress.Blazor.Reporting.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Timers;
using System.Linq;

namespace BlazorExportCustomization {
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();

            services.AddDevExpressServerSideBlazorReportViewer();
            services.AddScoped<IExportProcessor, CustomExportProcessor>();
            services.AddSingleton<CustomExportStorage, CustomExportStorage>();
            services.AddHttpContextAccessor();
        }

        void ClearExportResults(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                var filesForRemove = Directory.EnumerateFiles(directoryPath).Where(x =>
                    DateTime.UtcNow.Subtract(File.GetLastAccessTimeUtc(x)).TotalMilliseconds >= new TimeSpan(0, 5, 0).TotalMilliseconds
                );
                foreach (var filePath in filesForRemove)
                {
                    File.Delete(filePath);
                }
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseDevExpressServerSideBlazorReportViewer();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            var exportDirectoryPath = Path.Join(env.ContentRootPath, "temp", "export_results");
            ClearExportResults(exportDirectoryPath);
            var timer = new Timer();
            timer.Interval = new TimeSpan(0, 10, 0).TotalMilliseconds;
            timer.Elapsed += (s, e) => {
                ClearExportResults(exportDirectoryPath);
            };

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapControllers();
            });
        }
    }
}
