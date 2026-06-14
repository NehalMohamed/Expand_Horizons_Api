using ESC_POS_USB_NET.Printer;
using ITravel_App.Models;
using QuestPDF.Fluent;
using System.Drawing.Printing;


namespace ITravel_App.Services.PrinterService
{
    public class PrinterService : IPrinterService
    {
        private readonly IWebHostEnvironment _env;

        public PrinterService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task PrintBookingTicketAsync(
            BookingTicketDto ticket)
        {
            var document =
                new BookingTicketDocument(
                    ticket,
                    Path.Combine(
                        _env.WebRootPath,
                        "images",
                        "logo.png"));

            var pdfBytes = document.GeneratePdf();

            string tempFile =
                Path.Combine(
                    Path.GetTempPath(),
                    $"ticket_{Guid.NewGuid()}.pdf");

            await File.WriteAllBytesAsync(
                tempFile,
                pdfBytes);

            PrintPdf(tempFile);

            File.Delete(tempFile);
        }

        private void PrintPdf(string pdfPath)
        {
            //using var document =
            //    PdfiumViewer.PdfDocument.Load(pdfPath);

            //using var printDocument =
            //    document.CreatePrintDocument();

            //printDocument.PrinterSettings.PrinterName =
            //    "XP-Q807K";

            //printDocument.PrintController =
            //    new StandardPrintController();

            //printDocument.Print();
        }
        public async Task PrintBookingAsync(
           BookingTicketDto ticket)
        {
            string fallbackPrinter = "XP-Q807K";
            string logoPath =
                Path.Combine(
                    _env.WebRootPath,
                    "images",
                    "logo.png");

            var generator =
                new TicketImageGenerator();

            byte[] imageBytes =
                generator.Generate(
                    ticket,
                    logoPath);

            string tempImage =
                Path.Combine(
                    Path.GetTempPath(),
                    $"{Guid.NewGuid()}.png");

            await File.WriteAllBytesAsync(
                tempImage,
                imageBytes);

            //  var printer = new Printer("XP-80C");
            //new Printer("XP-Q807K");
            var printerName =
    PrinterSettings
        .InstalledPrinters
        .Cast<string>()
        .FirstOrDefault(x =>
            x.Contains("XP",
                StringComparison.OrdinalIgnoreCase));

            // Check if printer exists
           
            //if (string.IsNullOrEmpty(printerName))
            //{
            //    printerName = fallbackPrinter;
            //}

            var printer = new Printer(printerName);
            printer.InitializePrint();

            printer.Image(tempImage);

            printer.Append("\n");
            printer.Append("\n");

            printer.FullPaperCut();

            printer.PrintDocument();

            File.Delete(tempImage);
        }
    }
}
