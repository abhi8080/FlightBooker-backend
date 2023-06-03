using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Controllers
{
    /// <summary>
    /// Controller for handling airport-related operations.
    /// </summary>
    [ApiController]
    [Route("airports")]
    public class AirportController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="AirportController"/> class.
        /// </summary>
        /// <param name="db">The database context.</param>
        public AirportController(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Retrieves all airports.
        /// </summary>
        /// <returns>A collection of airports.</returns>
        [HttpGet]
        public async Task<IEnumerable<Airport>> GetAll()
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var airports = await _db.Airports.ToListAsync();
                    
                    await transaction.CommitAsync();

                    return airports;
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
