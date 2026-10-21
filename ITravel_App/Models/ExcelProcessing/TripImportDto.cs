namespace ITravel_App.Models.ExcelProcessing
{
    public class TripImportDto
    {
        public decimal Price { get; set; }

        public string TripName { get; set; } = string.Empty;

        public List<string> Highlights { get; set; } = [];

        public List<string> TripIncludes { get; set; } = [];

        public string Description { get; set; } = string.Empty;

        public string Destination { get; set; } = string.Empty;

        public List<string> TripNotIncludes { get; set; } = [];

        public List<string> DonnotForget { get; set; } = [];
        public List<string> AdditionalInfo { get; set; } = [];
    }
}
