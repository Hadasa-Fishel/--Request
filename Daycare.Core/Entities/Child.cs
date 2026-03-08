namespace Daycare.Core.Entities
{
    public class Child
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public int GroupId { get; set; }

        public string? ParentName { get; set; }

        public string? ParentPhone { get; set; }

        public string? ParentEmail { get; set; }

        public string? EmergencyContact { get; set; }

        public string? EmergencyPhone { get; set; }

        public string? MedicalNotes { get; set; }

        public string? Allergies { get; set; }

        public DateTime EnrollmentDate { get; set; }

        public bool IsActive { get; set; } = true;

        // Computed property - גיל הילד
        public int AgeInMonths
        {
            get
            {
                var today = DateTime.Today;
                var months = (today.Year - DateOfBirth.Year) * 12 + today.Month - DateOfBirth.Month;
                if (today.Day < DateOfBirth.Day)
                {
                    months--;
                }
                return months;
            }
        }

        public string FullName => $"{FirstName} {LastName}";
    }
}