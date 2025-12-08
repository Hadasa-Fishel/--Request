using DaycareManagementApi.Entities;

namespace DaycareManagementApi.Data
{
    public interface IDataContext
    {
        List<Child> Children { get; set; }
        List<Staff> Staff { get; set; }
        List<Group> Groups { get; set; }
        List<Attendance> Attendances { get; set; }
    }
}