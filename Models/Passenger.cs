using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class Passenger
{
    public int Id { get; set; }

    [ForeignKey("Booking")]
    public int BookingId { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }
    
    public virtual Booking? Booking { get; set; }

}