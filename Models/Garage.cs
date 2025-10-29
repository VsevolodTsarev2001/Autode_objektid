namespace Autode_objektid.Models
{
    public class Garage
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int? AddressId { get; set; }
        public Address? Address { get; set; }
    }
}
