using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartParking.API.Models
{
    public class ParkingSlot
    {
        [Key]
        public int SlotId { get; set; }
        
        [Required]
        public int ZoneId { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string SlotNumber { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Available"; // Available, Occupied, Reserved, Maintenance
        
        [ForeignKey("ZoneId")]
        public ParkingZone? Zone { get; set; }
    }
}