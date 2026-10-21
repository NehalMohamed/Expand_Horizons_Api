namespace ITravel_App.Models.ExcelProcessing
{
    public class TripImportResult
    {
        public int Imported { get; set; }

        public List<string> Errors { get; set; } = [];
    }
}
