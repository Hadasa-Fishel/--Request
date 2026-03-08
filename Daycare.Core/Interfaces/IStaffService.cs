using Daycare.Core.Entities;

namespace Daycare.Core.Interfaces
{
    public interface IStaffService
    {
        IEnumerable<Staff> GetStaff();
        Staff GetStaffMember(int id);
        Staff AddStaff(Staff newStaff);
        bool UpdateStaff(int id, Staff updatedStaff);
        bool DeleteStaff(int id);
        IEnumerable<Staff> GetStaffByPosition(string position);
    }
}