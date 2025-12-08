namespace DaycareManagementApi.Entities
{
    public class Staff
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string? Email { get; set; }

        public DateTime? HireDate { get; set; }

        public string? Address { get; set; }

        public string? IdNumber { get; set; }

        public decimal? Salary { get; set; }

        public int? AssignedGroupId { get; set; }

        public string? Qualifications { get; set; }

        public string? Notes { get; set; }

        public bool IsActive { get; set; } = true;

        public string FullName => $"{FirstName} {LastName}";

        // Computed property - ותק בעבודה
        public int? YearsOfService
        {
            get
            {
                if (HireDate == null) return null;
                return DateTime.Today.Year - HireDate.Value.Year;
            }
        }
    }
}