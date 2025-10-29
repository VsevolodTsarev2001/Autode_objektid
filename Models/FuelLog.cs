namespace Autode_objektid.Models
{
    public class FuelLog
    {
        public int Id { get; set; }

        public int CarId { get; set; }
        public Car? Car { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        // liters filled
        public double Liters { get; set; }

        // price per liter (eur)
        public double PricePerLiter { get; set; }

        // odometer reading (optional)
        public double? OdometerKm { get; set; }
    }
}
