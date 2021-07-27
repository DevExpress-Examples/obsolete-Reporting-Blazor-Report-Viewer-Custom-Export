using DevExpress.Blazor.Reporting.Models;
using DevExpress.Blazor.Reporting.Services;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BlazorExportCustomization.CustomExport
{
    public class CustomExportProcessor : IExportProcessor {
        readonly CustomExportStorage exportStorage;
        readonly string exportDirectoryPath;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAntiforgery _antiforgery;
        public CustomExportProcessor(CustomExportStorage exportStorage, 
            IWebHostEnvironment env, 
            IHttpContextAccessor httpContextAccessor, 
            IAntiforgery antiforgery) 
        {
            this.exportStorage = exportStorage;
            exportDirectoryPath = Path.Join(env.ContentRootPath, "temp", "export_results");
            _httpContextAccessor = httpContextAccessor;
            _antiforgery = antiforgery;
        }
        public async Task FinalizeStreamAsync(ExportResultItem item, Stream stream) {
            await stream.DisposeAsync();
        }
        public Task<Stream> GetStreamAsync(ExportResultItem item) {
            var exportStorageItem = new CustomExportStorageItem {
                ExportResultItem = item,
                FilePath = Path.Combine(exportDirectoryPath, item.Id),
            };
            exportStorage.Add(exportStorageItem);
            if(!Directory.Exists(exportDirectoryPath))
                Directory.CreateDirectory(exportDirectoryPath);
            return Task.FromResult<Stream>(File.Create(exportStorageItem.FilePath));
        }
        public Task<ExportResultRequestData> GetExportInfoAsync(ExportResultItem item) {
            return Task.FromResult(new ExportResultRequestData() {
                RequestUrl = "/GetExportResultFromCustomStorage",
                FormData = new Dictionary<string, string>() {
                    ["Id"] = item.Id,
                    ["__RequestVerificationToken"] = 
                    _antiforgery.GetAndStoreTokens(_httpContextAccessor.HttpContext).RequestToken
                }
            });
        }
    }
}
