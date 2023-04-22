using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class Booking
{
    public int Id { get; set; }

    [ForeignKey("Flight")]
    public int FlightId { get; set; }
    
    public virtual Flight? Flight { get; set; }

}