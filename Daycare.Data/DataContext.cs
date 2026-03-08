
using Daycare.Core.Entities;
using Daycare.Core.Interfaces;
namespace Daycare.Data
{
    public class DataContext : IDataContext
    {
        public List<Child> Children { get; set; }
        public List<Staff> Staff { get; set; }
        public List<Group> Groups { get; set; }
        public List<Attendance> Attendances { get; set; }

        // Constructor - אתחול נתונים ראשוניים
        public DataContext()
        {
            // אתחול קבוצות
            Groups = new List<Group>
            {
                new Group
                {
                    Id = 101,
                    Name = "תינוקות",
                    AgeRange = "3-12 חודשים",
                    MaxCapacity = 8,
                    CurrentCapacity = 2,
                    RoomNumber = "א1"
                },
                new Group
                {
                    Id = 102,
                    Name = "פעוטות",
                    AgeRange = "1-2 שנים",
                    MaxCapacity = 12,
                    CurrentCapacity = 2,
                    RoomNumber = "ב2"
                },
                new Group
                {
                    Id = 103,
                    Name = "גן",
                    AgeRange = "2-3 שנים",
                    MaxCapacity = 15,
                    CurrentCapacity = 0,
                    RoomNumber = "ג3"
                }
            };

            // אתחול ילדים
            Children = new List<Child>
            {
                new Child
                {
                    Id = 1,
                    FirstName = "נועה",
                    LastName = "כהן",
                    DateOfBirth = new DateTime(2023, 5, 10),
                    GroupId = 101,
                    ParentName = "דוד כהן",
                    ParentPhone = "050-1234567",
                    EnrollmentDate = new DateTime(2023, 9, 1)
                },
                new Child
                {
                    Id = 2,
                    FirstName = "אורי",
                    LastName = "לוי",
                    DateOfBirth = new DateTime(2022, 11, 25),
                    GroupId = 102,
                    ParentName = "רחל לוי",
                    ParentPhone = "050-7654321",
                    EnrollmentDate = new DateTime(2023, 9, 1)
                }
            };

            // אתחול צוות
            Staff = new List<Staff>
            {
                new Staff
                {
                    Id = 10,
                    FirstName = "שרה",
                    LastName = "ישראלי",
                    Position = "גננת ראשית",
                    PhoneNumber = "050-1111111",
                    HireDate = new DateTime(2020, 1, 1),
                    AssignedGroupId = 101
                },
                new Staff
                {
                    Id = 11,
                    FirstName = "רבקה",
                    LastName = "כהן",
                    Position = "סייעת",
                    PhoneNumber = "050-2222222",
                    HireDate = new DateTime(2021, 9, 1),
                    AssignedGroupId = 102
                }
            };

            // אתחול נוכחות
            Attendances = new List<Attendance>
            {
                new Attendance
                {
                    Id = 1,
                    ChildId = 1,
                    Date = DateTime.Today,
                    CheckInTime = new TimeSpan(8, 0, 0),
                    CheckOutTime = new TimeSpan(16, 30, 0),
                    IsPresent = true,
                    BroughtByName = "דוד כהן",
                    PickedUpByName = "דוד כהן"
                },
                new Attendance
                {
                    Id = 2,
                    ChildId = 2,
                    Date = DateTime.Today,
                    CheckInTime = new TimeSpan(7, 45, 0),
                    CheckOutTime = null,
                    IsPresent = true,
                    BroughtByName = "רחל לוי"
                }
            };
        }
    }
}