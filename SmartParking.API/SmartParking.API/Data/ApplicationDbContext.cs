using Microsoft.EntityFrameworkCore;
using SmartParking.API.Models;

namespace SmartParking.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<ParkingZone> ParkingZones { get; set; }
        public DbSet<ParkingSlot> ParkingSlots { get; set; }
        public DbSet<ParkingTransaction> ParkingTransactions { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Seed initial admin user (password: Admin123!)
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    Role = "Admin",
                    IsActive = true
                },
                new User
                {
                    UserId = 2,
                    Username = "operator",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Operator123!"),
                    Role = "Operator",
                    IsActive = true
                },
                new User
                {
                    UserId = 3,
                    Username = "viewer",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Viewer123!"),
                    Role = "Viewer",
                    IsActive = true
                }
            );
            
            // Seed parking zones
            modelBuilder.Entity<ParkingZone>().HasData(
                new ParkingZone { ZoneId = 1, ZoneName = "A", Description = "Zone A - Main Building" },
                new ParkingZone { ZoneId = 2, ZoneName = "B", Description = "Zone B - West Wing" },
                new ParkingZone { ZoneId = 3, ZoneName = "C", Description = "Zone C - East Wing" }
            );
            
            // Seed parking slots (10 per zone)
            for (int zoneId = 1; zoneId <= 3; zoneId++)
            {
                for (int slotNum = 1; slotNum <= 10; slotNum++)
                {
                    modelBuilder.Entity<ParkingSlot>().HasData(
                        new ParkingSlot
                        {
                            SlotId = (zoneId - 1) * 10 + slotNum,
                            ZoneId = zoneId,
                            SlotNumber = $"{(char)(64 + zoneId)}{slotNum}",
                            Status = "Available"
                        }
                    );
                }
            }
        }
    }
}