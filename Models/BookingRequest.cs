using System.ComponentModel.DataAnnotations;

namespace backend.Models;
//

public class BookingRequest
{
    public int FlightId { get; set; }


    [Required]
    public string[] passengerNames { get; set; }

    [Required]
    public string[] passengerEmails { get; set; }

}
