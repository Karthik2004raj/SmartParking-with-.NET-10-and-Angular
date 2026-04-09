namespace SmartParking.API.DTOs
{
    public class DashboardStatsDto
    {
        public int TotalSlots { get; set; }
        public int OccupiedSlots { get; set; }
        public int AvailableSlots { get; set; }
        public int ReservedSlots { get; set; }
        public int MaintenanceSlots { get; set; }
        public double UtilizationPercentage { get; set; }
        public string CongestionLevel { get; set; } = string.Empty;
        public int PredictedNextHourOccupancy { get; set; }
    }
}