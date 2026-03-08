namespace Daycare.Core.Entities
{
    public class Group
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string AgeRange { get; set; } = string.Empty;

        public int MaxCapacity { get; set; }

        public int CurrentCapacity { get; set; }

        public string RoomNumber { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int? MainStaffId { get; set; }

        public int? AssistantStaffId { get; set; }

        public TimeSpan? DailyStartTime { get; set; }

        public TimeSpan? DailyEndTime { get; set; }

        public string? Schedule { get; set; }

        public bool IsActive { get; set; } = true;

        // Computed property - מקומות פנויים
        public int AvailableSpots => MaxCapacity - CurrentCapacity;

        // Computed property - אחוז תפוסה
        public double OccupancyRate
        {
            get
            {
                if (MaxCapacity == 0) return 0;
                return Math.Round((double)CurrentCapacity / MaxCapacity * 100, 1);
            }
        }

        // Computed property - האם מלא
        public bool IsFull => CurrentCapacity >= MaxCapacity;
    }
}