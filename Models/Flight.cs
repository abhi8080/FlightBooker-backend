using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class Flight
{
    public int Id { get; set; }

    [ForeignKey("DepartureAirport")]
    public int DepartureAirportId { get; set; }

    [ForeignKey("ArrivalAirport")]
    public int ArrivalAirportId { get; set; }

    public DateTime StartDateTime { get; set; }

    public TimeSpan FlightDuration { get; set; }

    public virtual Airport? DepartureAirport { get; set; }

    public virtual Airport? ArrivalAirport { get; set; }
}
