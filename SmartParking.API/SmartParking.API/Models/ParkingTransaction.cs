using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartParking.API.Models
{
    public class ParkingTransaction
    {
        [Key]
        public int TransactionId { get; set; }
        
        [Required]
        public int SlotId { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string VehicleNumber { get; set; } = string.Empty;
        
        [Required]
        public DateTime EntryTime { get; set; }
        
        public DateTime? ExitTime { get; set; }
        
        public int? DurationMinutes { get; set; }
        
        [ForeignKey("SlotId")]
        public ParkingSlot? Slot { get; set; }
    }
}