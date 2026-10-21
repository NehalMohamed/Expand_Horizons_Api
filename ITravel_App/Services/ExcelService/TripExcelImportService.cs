using ClosedXML.Excel;
using ITravel_App.Models.ExcelProcessing;
using System.Linq;

namespace ITravel_App.Services.ExcelService
{
    public class TripExcelImportService
    {
        public async Task<(List<TripImportDto> Trips, List<string> Errors)>
        ReadExcelAsync(Stream stream)
        {
            var trips = new List<TripImportDto>();
            var errors = new List<string>();

            using var workbook = new XLWorkbook(stream);

            // Validate sheets
            var requiredSheets = new[]
 {
    "Trips",
    "Highlights",
    "Includes",
    "Not Includes","not Forget","Additional Information"
};

            foreach (var sheetName in requiredSheets)
            {
                if (!workbook.Worksheets.Any(ws =>
                    ws.Name.Equals(
                        sheetName,
                        StringComparison.OrdinalIgnoreCase)))
                {
                    errors.Add($"Missing worksheet: {sheetName}");
                }
            }

            if (errors.Count > 0)
            {
                return (trips, errors);
            }

            var tripsSheet = GetWorksheet(workbook, "Trips");
            var highlightsSheet = GetWorksheet(workbook, "Highlights");
            var includesSheet = GetWorksheet(workbook, "Includes");
            var notIncludesSheet = GetWorksheet(workbook, "Not Includes");
            var notForgetSheet = GetWorksheet(workbook, "not Forget");
            var additionalInformationSheet = GetWorksheet(workbook, "Additional Information");

            if (tripsSheet == null ||
                highlightsSheet == null ||
                includesSheet == null ||
                notIncludesSheet == null || notForgetSheet == null || additionalInformationSheet == null)
            {
                return (trips, errors);
            }

            // -----------------------------------------
            // Read Trips
            // -----------------------------------------

            var tripDictionary = new Dictionary<int, TripImportDto>();

            var tripRows = tripsSheet
                .RowsUsed()
                .Skip(1);

            foreach (var row in tripRows)
            {
                if (row.IsEmpty())
                    continue;

                var rowNumber = row.RowNumber();

                try
                {
                    var tripIdText = row.Cell(1).GetString().Trim();
                    var tripName = row.Cell(2).GetString().Trim();
                    var priceText = row.Cell(3).GetString().Trim();

                    if (string.IsNullOrWhiteSpace(tripIdText))
                    {
                        errors.Add($"Trips row {rowNumber}: TripId is required.");
                        continue;
                    }

                    if (!int.TryParse(tripIdText, out var tripId))
                    {
                        errors.Add(
                            $"Trips row {rowNumber}: TripId '{tripIdText}' is invalid."
                        );

                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(tripName))
                    {
                        errors.Add(
                            $"Trips row {rowNumber}: Trip Name is required."
                        );

                        continue;
                    }

                    if (!decimal.TryParse(
                            priceText,
                            System.Globalization.NumberStyles.Any,
                            System.Globalization.CultureInfo.InvariantCulture,
                            out var price))
                    {
                        errors.Add(
                            $"Trips row {rowNumber}: Price '{priceText}' is invalid."
                        );

                        continue;
                    }

                    if (price < 0)
                    {
                        errors.Add(
                            $"Trips row {rowNumber}: Price cannot be negative."
                        );

                        continue;
                    }

                    if (tripDictionary.ContainsKey(tripId))
                    {
                        errors.Add(
                            $"Trips row {rowNumber}: Duplicate TripId {tripId}."
                        );

                        continue;
                    }

                    var trip = new TripImportDto
                    {
                        Price = price,

                        TripName = tripName,

                        Description = row.Cell(4)
                            .GetString()
                            .Trim(),

                        Destination = row.Cell(5)
                            .GetString()
                            .Trim(),

                        //ImportantInfo = row.Cell(6)
                        //    .GetString()
                        //    .Trim()
                    };

                    tripDictionary.Add(tripId, trip);
                }
                catch (Exception ex)
                {
                    errors.Add(
                        $"Trips row {rowNumber}: {ex.Message}"
                    );
                }
            }

            // -----------------------------------------
            // Read Highlights
            // -----------------------------------------

            ReadListSheet(
                highlightsSheet,
                tripDictionary,
                errors,
                "Highlight",
                (trip, value) =>
                {
                    trip.Highlights.Add(value);
                });

            // -----------------------------------------
            // Read Includes
            // -----------------------------------------

            ReadListSheet(
                includesSheet,
                tripDictionary,
                errors,
                "Included Item",
                (trip, value) =>
                {
                    trip.TripIncludes.Add(value);
                });

            // -----------------------------------------
            // Read Not Includes
            // -----------------------------------------

            ReadListSheet(
                notIncludesSheet,
                tripDictionary,
                errors,
                "Not Included Item",
                (trip, value) =>
                {
                    trip.TripNotIncludes.Add(value);
                });


            // -----------------------------------------
            // Read Not Forget
            // -----------------------------------------

            ReadListSheet(
                notForgetSheet,
                tripDictionary,
                errors,
                "Important Item",
                (trip, value) =>
                {
                    trip.DonnotForget.Add(value);
                });


            // -----------------------------------------
            // Read Additional Info
            // -----------------------------------------

            ReadListSheet(
                additionalInformationSheet,
                tripDictionary,
                errors,
                "Additional Information Item",
                (trip, value) =>
                {
                    trip.AdditionalInfo.Add(value);
                });
            trips = tripDictionary.Values.ToList();

            return (trips, errors);
        }

        private static void ReadListSheet(
            IXLWorksheet worksheet,
            Dictionary<int, TripImportDto> trips,
            List<string> errors,
            string columnName,
            Action<TripImportDto, string> addValue)
        {
            foreach (var row in worksheet.RowsUsed().Skip(1))
            {
                if (row.IsEmpty())
                    continue;

                var rowNumber = row.RowNumber();

                var tripIdText = row.Cell(1)
                    .GetString()
                    .Trim();

                var value = row.Cell(2)
                    .GetString()
                    .Trim();

                if (string.IsNullOrWhiteSpace(tripIdText) &&
                    string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }

                if (!int.TryParse(tripIdText, out var tripId))
                {
                    errors.Add(
                        $"{worksheet.Name} row {rowNumber}: " +
                        $"Invalid TripId '{tripIdText}'."
                    );

                    continue;
                }

                if (!trips.TryGetValue(tripId, out var trip))
                {
                    errors.Add(
                        $"{worksheet.Name} row {rowNumber}: " +
                        $"TripId {tripId} does not exist in Trips."
                    );

                    continue;
                }

                if (string.IsNullOrWhiteSpace(value))
                {
                    errors.Add(
                        $"{worksheet.Name} row {rowNumber}: " +
                        $"{columnName} is required."
                    );

                    continue;
                }

                addValue(trip, value);
            }
        }

        private static IXLWorksheet? GetWorksheet(
        XLWorkbook workbook,
        string name)
        {
            return workbook.Worksheets.FirstOrDefault(ws =>
                ws.Name.Equals(
                    name,
                    StringComparison.OrdinalIgnoreCase));
        }

    }
}
