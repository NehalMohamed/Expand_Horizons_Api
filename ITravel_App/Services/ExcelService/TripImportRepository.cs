using ITravel_App.Models.ExcelProcessing;
using Npgsql;
using System.Text.Json;

namespace ITravel_App.Services.ExcelService
{
    public class TripImportRepository
    {
        private readonly IConfiguration _configuration;

        public TripImportRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task ImportTripsAsync(
            List<TripImportDto> trips)
        {
            var connectionString =
                _configuration.GetConnectionString("DefaultConnection");

            await using var connection =
                new NpgsqlConnection(connectionString);

            await connection.OpenAsync();

            await using var transaction =
                await connection.BeginTransactionAsync();

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                };

                var json = JsonSerializer.Serialize(
                    new
                    {
                        result = trips
                    },
                    options);
                //var json = JsonSerializer.Serialize(
                //    new
                //    {
                //        result = trips
                //    });

                await using var command =
                    new NpgsqlCommand(
                        "SELECT import_trips(@json::jsonb);",
                        connection,
                        transaction);

                command.Parameters.AddWithValue(
                    "json",
                    json);

                await command.ExecuteScalarAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();

                throw;
            }
        }
    }
}
