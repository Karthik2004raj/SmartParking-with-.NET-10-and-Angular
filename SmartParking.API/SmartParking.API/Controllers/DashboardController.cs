using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartParking.API.Data;
using SmartParking.API.DTOs;

namespace SmartParking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var totalSlots = await _context.ParkingSlots.CountAsync();
            var occupiedSlots = await _context.ParkingSlots.CountAsync(s => s.Status == "Occupied");
            var reservedSlots = await _context.ParkingSlots.CountAsync(s => s.Status == "Reserved");
            var maintenanceSlots = await _context.ParkingSlots.CountAsync(s => s.Status == "Maintenance");
            var availableSlots = totalSlots - occupiedSlots - reservedSlots - maintenanceSlots;
            
            var utilizationPercentage = totalSlots > 0 ? (double)occupiedSlots / totalSlots * 100 : 0;
            
            string congestionLevel = utilizationPercentage switch
            {
                >= 90 => "Critical",
                >= 80 => "High",
                >= 60 => "Moderate",
                >= 30 => "Low",
                _ => "Very Low"
            };
            
            // Simple prediction for next hour
            var predictedNextHour = PredictNextHourOccupancy(utilizationPercentage);
            
            return Ok(new DashboardStatsDto
            {
                TotalSlots = totalSlots,
                OccupiedSlots = occupiedSlots,
                AvailableSlots = availableSlots,
                ReservedSlots = reservedSlots,
                MaintenanceSlots = maintenanceSlots,
                UtilizationPercentage = Math.Round(utilizationPercentage, 2),
                CongestionLevel = congestionLevel,
                PredictedNextHourOccupancy = predictedNextHour
            });
        }
        
        [HttpGet("zones")]
        public async Task<IActionResult> GetZonesWithSlots()
        {
            var zones = await _context.ParkingZones
                .Include(z => z.ParkingSlots)
                .Select(z => new
                {
                    z.ZoneId,
                    z.ZoneName,
                    z.Description,
                    Slots = z.ParkingSlots.Select(s => new
                    {
                        s.SlotId,
                        s.SlotNumber,
                        s.Status
                    }),
                    TotalSlots = z.ParkingSlots.Count,
                    OccupiedSlots = z.ParkingSlots.Count(s => s.Status == "Occupied"),
                    AvailableSlots = z.ParkingSlots.Count(s => s.Status == "Available")
                })
                .ToListAsync();
            
            return Ok(zones);
        }
        
        private int PredictNextHourOccupancy(double currentOccupancy)
        {
            // Simple prediction formula
            var prediction = (currentOccupancy * 0.7) + (50 * 0.3); // Assume historical average of 50%
            return (int)Math.Min(100, Math.Round(prediction));
        }
    }
}
