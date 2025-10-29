using System.ComponentModel.DataAnnotations;

namespace Autode_objektid.Models
{
    public class Driver
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        // водительское удостоверение
        public string? LicenseNumber { get; set; }

        // optional FK
        public int? ContactInfoId { get; set; }
        public ContactInfo? ContactInfo { get; set; }
    }
}
