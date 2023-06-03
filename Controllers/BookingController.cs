using Microsoft.AspNetCore.Mvc;
using backend.Data;
using backend.Models;

namespace backend.Controllers
{
    /// <summary>
    /// Controller for handling booking-related operations.
    /// </summary>
    [ApiController]
    [Route("book")]
    public class BookingController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingController"/> class.
        /// </summary>
        /// <param name="db">The database context.</param>
        public BookingController(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Creates a new booking.
        /// </summary>
        /// <param name="request">The booking request.</param>
        /// <returns>An <see cref="IActionResult"/> representing the HTTP response.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(BookingRequest request)
        {
            using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    Booking booking = new Booking
                    {
                        FlightId = request.FlightId,
                    };

                    await _db.Bookings.AddAsync(booking);
                    await _db.SaveChangesAsync();

                    for (int i = 0; i < request.passengerNames.Length; i++)
                    {
                        string name = request.passengerNames[i];
                        string email = request.passengerEmails[i];

                        Passenger passenger = new Passenger
                        {
                            BookingId = booking.Id,
                            Name = name,
                            Email = email
                        };

                        await _db.Passengers.AddAsync(passenger);
                    }

                    await _db.SaveChangesAsync();

                    await transaction.CommitAsync(); 

                    return CreatedAtAction(null, null, booking.Id);
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
