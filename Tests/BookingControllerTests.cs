using backend.Controllers;
using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

public class BookingControllerTests {
    
    [Fact]
    public async Task Create_Returns_CreatedAtAction()
    {
        
        var booking = new Booking { FlightId = 1 };
        var passengerNames = new[] { "John", "Jane" };
        var passengerEmails = new[] { "john@example.com", "jane@example.com" };
        var request = new BookingRequest
        {
            FlightId = booking.FlightId,
            passengerNames = passengerNames,
            passengerEmails = passengerEmails
        };

        var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "BookingControllerTest")
            .Options;

        using (var dbContext = new ApplicationDbContext(dbContextOptions))
        {
            var controller = new BookingController(dbContext);

        
            var result = await controller.Create(request);

        
            Assert.IsType<CreatedAtActionResult>(result);
        }
    }
}
