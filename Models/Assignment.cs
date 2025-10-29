namespace Autode_objektid.Models
{
    public class Assignment
    {
        public int Id { get; set; }

        public int CarId { get; set; }
        public Car? Car { get; set; }

        public int DriverId { get; set; }
        public Driver? Driver { get; set; }

        // период назначения
        public DateTime From { get; set; } = DateTime.UtcNow;
        public DateTime? To { get; set; }
    }
}
