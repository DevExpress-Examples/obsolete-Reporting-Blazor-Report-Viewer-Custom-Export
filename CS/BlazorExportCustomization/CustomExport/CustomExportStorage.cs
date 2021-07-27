using DevExpress.Blazor.Reporting.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BlazorExportCustomization.CustomExport
{
    public class CustomExportStorageItem {
        public ExportResultItem ExportResultItem { get; set; }
        public string FilePath { get; set; }
    }

    public class CustomExportStorage {
        ConcurrentDictionary<string, CustomExportStorageItem> Items = 
            new ConcurrentDictionary<string, CustomExportStorageItem>();
        public void Add(CustomExportStorageItem item) {
            Items.TryAdd(item.ExportResultItem.Id, item);
        }

        public CustomExportStorageItem GetAndRemove(string id) {
            CustomExportStorageItem exportStorageItem;
            Items.Remove(id, out exportStorageItem);
            return exportStorageItem;
        }
    }
}
