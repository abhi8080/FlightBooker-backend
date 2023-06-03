using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using System.Globalization;
using Npgsql;

namespace backend.Controllers
{
    [ApiController]
    [Route("flights")]
    public class FlightController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlightController"/> class.
        /// </summary>
        /// <param name="db">The database context.</param>
        public FlightController(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Retrieves a list of distinct flight dates.
        /// </summary>
        /// <returns>A list of distinct flight dates.</returns>
        [HttpGet("dates")]
        public async Task<List<DateTime>> GetFlightDates()
        {
            using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    using var conn = new NpgsqlConnection(Env.connectionString);
                    await conn.OpenAsync();

                    string query = @"
                        SELECT DISTINCT date_trunc('day', ""StartDateTime"")
                        FROM ""Flights""";

                    using var command = new NpgsqlCommand(query, conn);

                    using var reader = await command.ExecuteReaderAsync();
                    var result = new List<DateTime>();
                    while (await reader.ReadAsync())
                    {
                        var date = reader.GetDateTime(0);
                        result.Add(date);
                    }

                    await transaction.CommitAsync();

                    return result;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        /// <summary>
        /// Retrieves a list of flights based on search criteria.
        /// </summary>
        /// <param name="departureAirportCode">The departure airport code.</param>
        /// <param name="arrivalAirportCode">The arrival airport code.</param>
        /// <param name="startDate">The start date of the flights.</param>
        /// <returns>A list of flights matching the search criteria.</returns>
        [HttpGet]
        public async Task<List<Flight>> GetFlightsBySearchCriteria([FromQuery] int departureAirportCode, [FromQuery] int arrivalAirportCode, [FromQuery] string startDate)
        {
            using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    DateTime date;
                    DateTime.TryParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

                    var utcDateTime = DateTime.SpecifyKind(new DateTime(date.Year, date.Month, date.Day), DateTimeKind.Utc);

                    var results = await _db.Flights
                        .Where(f => f.StartDateTime.Date == utcDateTime &&
                                    f.DepartureAirportId == departureAirportCode &&
                                    f.ArrivalAirportId == arrivalAirportCode)
                        .Select(f => new
                        {
                            Id = f.Id,
                            DepartureAirportId = f.DepartureAirportId,
                            ArrivalAirportId = f.ArrivalAirportId,
                            FlightDuration = f.FlightDuration,
                            StartDateTime = f.StartDateTime
                        })
                        .ToListAsync();

                    await transaction.CommitAsync();

                    return results.Select(r => new Flight
                    {
                        Id = r.Id,
                        DepartureAirport = _db.Airports.FirstOrDefault(a => a.Id == r.DepartureAirportId),
                        ArrivalAirport = _db.Airports.FirstOrDefault(a => a.Id == r.ArrivalAirportId),
                        FlightDuration = r.FlightDuration,
                        StartDateTime = r.StartDateTime
                    }).ToList();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}