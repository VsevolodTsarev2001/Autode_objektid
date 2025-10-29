// Models/TripLog.cs
namespace Autode_objektid.Models
{
    public class TripLog
    {
        public int Id { get; set; }

        public int CarId { get; set; }
        public Car? Car { get; set; }

        public DateTime Timestamp { get; set; }
        public double DistanceKm { get; set; } // positive for edasi, negative for tagasi
    }
}
