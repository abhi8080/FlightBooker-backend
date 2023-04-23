using backend.Controllers;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class FlightControllerTests
{

    private readonly List<Airport> _airports;
    private readonly List<Flight> _flights;

    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;


    public FlightControllerTests()
    {
         _airports = new List<Airport>
        {
            new Airport { AirportCode = "LAX" },
            new Airport { AirportCode = "JFK" },
            new Airport { AirportCode = "SFO" },
            new Airport { AirportCode = "ORD" },
            new Airport { AirportCode = "DFW" }
        };

        _flights = new List<Flight>
        {
            new Flight { DepartureAirportId = 1, ArrivalAirportId = 2, StartDateTime = new DateTime(2022, 5, 1, 10, 0, 0), FlightDuration = new TimeSpan(2, 0, 0) },
            new Flight { DepartureAirportId = 3, ArrivalAirportId = 4, StartDateTime = new DateTime(2022, 5, 2, 11, 0, 0), FlightDuration = new TimeSpan(2, 30, 0) },
            new Flight { DepartureAirportId = 2, ArrivalAirportId = 3, StartDateTime = new DateTime(2022, 5, 1, 12, 0, 0), FlightDuration = new TimeSpan(1, 30, 0) },
            new Flight { DepartureAirportId = 4, ArrivalAirportId = 1, StartDateTime = new DateTime(2022, 5, 5, 13, 0, 0), FlightDuration = new TimeSpan(3, 0, 0) },
            new Flight { DepartureAirportId = 5, ArrivalAirportId = 1, StartDateTime = new DateTime(2022, 5, 5, 14, 0, 0), FlightDuration = new TimeSpan(4, 0, 0) }
        };

        _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }



   [Fact]
public async Task GetFlightDates_Returns_FlightDates()
{      
       using (var dbContext = new ApplicationDbContext(_dbContextOptions))
        {
            dbContext.Airports.AddRange(_airports);
            dbContext.Flights.AddRange(_flights);
            dbContext.SaveChanges();

        var controller = new FlightController(dbContext);
        var result = await controller.GetFlightDates();

        Assert.IsType<List<DateTime>>(result);
        Assert.Equal(3, result.Count());
        }
        
}

[Fact]
public async Task GetFlightsBySearchCriteria_Returns_FlightsMatchingCriteria()
{
    using (var dbContext = new ApplicationDbContext(_dbContextOptions))
        {
            dbContext.Airports.AddRange(_airports);
            dbContext.Flights.AddRange(_flights);
            dbContext.SaveChanges();

          var controller = new FlightController(dbContext);

    // Act
    var result = await controller.GetFlightsBySearchCriteria(1, 2, "2022-05-01");

    // Assert
    Assert.IsType<List<Flight>>(result);
    Assert.Equal(1, result.Count());
    Assert.Equal(1, result[0].Id);
    Assert.Equal(_airports[0], result[0].DepartureAirport);
    Assert.Equal(_airports[1], result[0].ArrivalAirport);
    Assert.Equal(new TimeSpan(2, 0, 0), result[0].FlightDuration);
    Assert.Equal(new DateTime(2022, 5, 1, 10, 0, 0), result[0].StartDateTime);
        }
    
}

[Fact]
public async Task GetFlightsBySearchCriteria_Returns_EmptyListWhenNoMatches()
{
    using (var dbContext = new ApplicationDbContext(_dbContextOptions))
        {
            dbContext.Airports.AddRange(_airports);
            dbContext.Flights.AddRange(_flights);
            dbContext.SaveChanges();
  // Arrange
    var controller = new FlightController(dbContext);

    // Act
    var result = await controller.GetFlightsBySearchCriteria(1, 2, "2022-05-02");

    // Assert
    Assert.IsType<List<Flight>>(result);
    Assert.Empty(result);
        }
  
}


}
