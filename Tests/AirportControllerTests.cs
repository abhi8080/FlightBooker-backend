using backend.Controllers;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class AirportControllerTests
{
    [Fact]
    public async Task GetAll_Returns_All_Airports()
    {
         var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "AirportControllerTest")
            .Options;
        Airport airport1 = new Airport { Id = 1, AirportCode = "SFO" };
        Airport airport2 = new Airport { Id = 2, AirportCode = "SFL" };


        using (var dbContext = new ApplicationDbContext(dbContextOptions))
        {
            dbContext.Airports.Add(airport1);
            dbContext.Airports.Add(airport2);
            await dbContext.SaveChangesAsync();

            var controller = new AirportController(dbContext);
            var result = await controller.GetAll();

        Assert.IsType<List<Airport>>(result);
        Assert.Equal(2, result.Count());


        }
    }
}
