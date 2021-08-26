<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/389625342/2021.1)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T1020313)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
# Report Viewer for Blazor - Custom Export

This project demonstrates how to use an API to customize the Report Viewer's 'native' server-side Blazor component.

## Overview
The Report Viewer API entry point is the [DxReportViewer](https://docs.devexpress.com/XtraReports/DevExpress.Blazor.Reporting.DxReportViewer) class. You can specify its properties and handle its events to adjust the Report Viewer appearance and behavior. 

## Example: Specify Export Options
Related page: [Index.razor](CS/BlazorExportCustomization/Pages/Index.razor).

The sample code sets the exported image format, and export mode, and specifies the page range for PDF export.

The example handles the [XtraReport.AfterPrint](https://docs.devexpress.com/XtraReports/DevExpress.XtraReports.UI.XRControl.AfterPrint) event and uses the [PrintingSystem.ExportOptions](https://docs.devexpress.com/CoreLibraries/DevExpress.XtraPrinting.PrintingSystemBase.ExportOptions) property to specify export options.

## Example: Remove Unnecessary Formats from the Toolbar Export Commands
Related page: [Index.razor](CS/BlazorExportCustomization/Pages/Index.razor).

The sample code retains toolbar commands that export a document to PDF and Image format (specified in the export options).

The example handles the [DxReportViewer.OnCustomizeToolbar](https://docs.devexpress.com/XtraReports/DevExpress.Blazor.Reporting.DxReportViewer.OnCustomizeToolbar) event and modifies items in the [ToolbarModel.AllItems](https://docs.devexpress.com/XtraReports/DevExpress.Blazor.Reporting.Models.ToolbarModel.AllItems) collection.

## Example: Handle Export Actions and Token-Based Authentication
Related pages: 
- [Startup.cs](CS/BlazorExportCustomization/Startup.cs)
- [CustomExportController.cs](CS/BlazorExportCustomization/CustomExport/CustomExportController.cs)
- [CustomExportProcessor.cs](CS/BlazorExportCustomization/CustomExport/CustomExportProcessor.cs)
- [CustomExportStorage.cs](CS/BlazorExportCustomization/CustomExport/CustomExportStorage.cs)

The sample code registers two custom services (CustomExportProcessor and CustomExportStorage) to manage export requests and temporarily store exported documents. The CustomExportProcessor service, which implements the [IExportProcessor](https://docs.devexpress.com/XtraReports/DevExpress.Blazor.Reporting.Services.IExportProcessor) interface, gets the exported document, saves it to a temporary folder and registers the document in the custom storage. After that, the service calls a custom controller action that creates an HTTP response with an anti-forgery token. The CustomExportStorage service maintains a dictionary of custom document locations and export action identifiers. 

The process that cleans up the temporary folder is started at application startup and triggered by timer.

## How to Run the Example

1. Download the project and update the **DevExpress.Blazor.Reporting.Viewer** and **DevExpress.Blazor** NuGet packages.
2. Build and run the project.
