using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using System.Globalization;

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
public async Task<IEnumerable<DateTime>> GetFlightDates()
{
    return await _db.Flights
        .Select(f => f.StartDateTime.Date)
        .Distinct()
        .ToListAsync();
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
