namespace DaycareManagementApi.Entities
{
    public class Attendance
    {
        public int Id { get; set; }

        public int ChildId { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan CheckInTime { get; set; }

        public TimeSpan? CheckOutTime { get; set; }

        public bool IsPresent { get; set; } = true;

        public string? Notes { get; set; }

        public string? BroughtByName { get; set; }

        public string? PickedUpByName { get; set; }

        public string? MoodAtArrival { get; set; }

        public bool? AteBreakfast { get; set; }

        public bool? AteLunch { get; set; }

        public bool? AteSnack { get; set; }

        public int? NapDurationMinutes { get; set; }

        public int? DiaperChanges { get; set; }

        public string? DailyReport { get; set; }

        // Computed property - משך השהייה במעון
        public TimeSpan? StayDuration
        {
            get
            {
                if (CheckOutTime == null) return null;
                return CheckOutTime.Value - CheckInTime;
            }
        }

        // Computed property - האם הילד עדיין במעון
        public bool IsCurrentlyAtDaycare => IsPresent && CheckOutTime == null;

        // Computed property - סטטוס נוכחות
        public string AttendanceStatus
        {
            get
            {
                if (!IsPresent) return "לא נוכח";
                if (CheckOutTime == null) return "במעון";
                return "נאסף";
            }
        }
    }
}