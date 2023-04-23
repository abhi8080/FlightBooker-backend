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

        public FlightController(ApplicationDbContext db)
        {
            _db = db;
        }



[HttpGet("dates")]
   public async Task<List<DateTime>> GetFlightDates()
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

        return result;
    }


[HttpGet]
public async Task<List<Flight>> GetFlightsBySearchCriteria([FromQuery] int departureAirportCode, [FromQuery] int arrivalAirportCode, [FromQuery] string startDate)
{
    DateTime date;
    DateTime.TryParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture,DateTimeStyles.None, out date);

    var utcDateTime = DateTime.SpecifyKind(new DateTime(date.Year, date.Month, date.Day), DateTimeKind.Utc);

   var results = await _db.Flights
    .Where(f => f.StartDateTime.Date == utcDateTime &&
                f.DepartureAirportId == departureAirportCode &&
                f.ArrivalAirportId == arrivalAirportCode)
    .Select(f => new {
        Id = f.Id,
        DepartureAirportId = f.DepartureAirportId,
        ArrivalAirportId = f.ArrivalAirportId,
        FlightDuration = f.FlightDuration,
        StartDateTime = f.StartDateTime
    })
    .ToListAsync();

return results.Select(r => new Flight {
    Id = r.Id, 
    DepartureAirport = _db.Airports.FirstOrDefault(a => a.Id == r.DepartureAirportId),
    ArrivalAirport = _db.Airports.FirstOrDefault(a => a.Id == r.ArrivalAirportId),
    FlightDuration = r.FlightDuration,
    StartDateTime = r.StartDateTime
}).ToList();

}

    }
}
