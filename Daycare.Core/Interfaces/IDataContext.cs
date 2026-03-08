using Daycare.Core.Entities;

namespace Daycare.Core.Interfaces
{
    public interface IDataContext
    {
        List<Child> Children { get; set; }
        List<Staff> Staff { get; set; }
        List<Group> Groups { get; set; }
        List<Attendance> Attendances { get; set; }
    }
}