namespace Autode_objektid.Models
{
    public class MaintenanceRecord
    {
        public int Id { get; set; }

        public int CarId { get; set; }
        public Car? Car { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        public string Description { get; set; } = string.Empty;

        // eur
        public double Cost { get; set; }
    }
}
