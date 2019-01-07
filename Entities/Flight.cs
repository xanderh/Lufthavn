using System;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Flight
    {
        [Key]
        public int FlightId { get; set; }
        public string AircraftType { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        [Timestamp]
        public Byte[] Version { get; set; }
    }
}
