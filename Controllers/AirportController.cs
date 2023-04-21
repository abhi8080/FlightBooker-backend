using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Controllers
{
    [ApiController]
    [Route("airports")]
    public class AirportController : ControllerBase
    {

         private readonly ApplicationDbContext _db;

        public AirportController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IEnumerable<Airport>> GetAll()
        {
            return await _db.Airports.ToListAsync();
        }
    }
}
