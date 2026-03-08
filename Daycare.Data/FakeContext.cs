using Daycare.Core.Entities;
using Daycare.Core.Interfaces;

namespace Daycare.Data
{
    public class FakeContext : IDataContext
    {
        public List<Child> Children { get; set; }
        public List<Staff> Staff { get; set; }
        public List<Group> Groups { get; set; }
        public List<Attendance> Attendances { get; set; }

        // Constructor - נתוני טסט מינימליים
        public FakeContext()
        {
            Groups = new List<Group>
            {
                new Group
                {
                    Id = 1,
                    Name = "Test Group",
                    AgeRange = "1-2",
                    MaxCapacity = 10,
                    CurrentCapacity = 1,
                    RoomNumber = "T1"
                }
            };

            Children = new List<Child>
            {
                new Child
                {
                    Id = 1,
                    FirstName = "Test",
                    LastName = "Child",
                    DateOfBirth = DateTime.Today.AddYears(-1),
                    GroupId = 1,
                    ParentName = "Test Parent",
                    ParentPhone = "050-0000000",
                    EnrollmentDate = DateTime.Today
                }
            };

            Staff = new List<Staff>
            {
                new Staff
                {
                    Id = 1,
                    FirstName = "Test",
                    LastName = "Staff",
                    Position = "Teacher",
                    PhoneNumber = "050-0000000"
                }
            };

            Attendances = new List<Attendance>
            {
                new Attendance
                {
                    Id = 1,
                    ChildId = 1,
                    Date = DateTime.Today,
                    CheckInTime = new TimeSpan(8, 0, 0),
                    IsPresent = true
                }
            };
        }
    }
}