using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using Newtonsoft.Json.Linq;
using backend.Models;
using System.Globalization;



namespace backend.Controllers
{
    [ApiController]
    [Route("book")]
    public class BookingController : ControllerBase
    {

         private readonly ApplicationDbContext _db;

        public BookingController(ApplicationDbContext db)
        {
            _db = db;
        }

[HttpPost]
public async Task<IActionResult> Create(BookingRequest request)
{
        Booking booking = new Booking {
            FlightId = request.FlightId,
        };

        await _db.Bookings.AddAsync(booking);
        await _db.SaveChangesAsync();
      

    for (int i = 0; i < request.passengerNames.Length; i++)
     {
       string name = request.passengerNames[i];
       string email = request.passengerEmails[i];

        Passenger passenger = new Passenger {
            BookingId = booking.Id,
            Name = name,
            Email = email
        };

        await _db.Passengers.AddAsync(passenger);
    }

     await _db.SaveChangesAsync();

     return CreatedAtAction(null, null, booking.Id);

}

    }
}
