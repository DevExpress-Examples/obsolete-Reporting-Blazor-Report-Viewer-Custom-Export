using System.IO;
using Microsoft.AspNetCore.Mvc;

using DevExpress.Blazor.Reporting.Controllers;
using DevExpress.Blazor.Reporting.Internal.Services;

namespace BlazorExportCustomization.CustomExport
{
    [ApiController, Route("GetExportResultFromCustomStorage")]
    [AutoValidateAntiforgeryToken]
    public class CustomExportController : ControllerBase
    {
        readonly CustomExportStorage exportResultStorage;
        public CustomExportController(CustomExportStorage exportResultStorage)
        {
            this.exportResultStorage = exportResultStorage;
        }

        [HttpPost]
        public IActionResult Post([FromForm] string Id)
        {
            var item = exportResultStorage.GetAndRemove(Id);
            if (item.ExportResultItem.InlineResult)
            {
                return File(System.IO.File.Open(item.FilePath, FileMode.Open),
                    item.ExportResultItem.ContentType);
            }
            return File(System.IO.File.Open(item.FilePath, FileMode.Open),
                item.ExportResultItem.ContentType,
                item.ExportResultItem.FileName);
        }
    }
}
