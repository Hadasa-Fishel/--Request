using Daycare.Core;
using Daycare.Core.Entities;
using Daycare.Core.Interfaces;

namespace Daycare.Service
{
    public class StaffService : IStaffService
    {
        private readonly IDataContext _context;
        private static int _nextId = 12;

        public StaffService(IDataContext context)
        {
            _context = context;
        }

        public IEnumerable<Staff> GetStaff()
        {
            return _context.Staff.Where(s => s.IsActive);
        }

        public Staff GetStaffMember(int id)
        {
            return _context.Staff.FirstOrDefault(s => s.Id == id && s.IsActive);
        }

        public Staff AddStaff(Staff newStaff)
        {
            newStaff.Id = _nextId++;
            newStaff.HireDate = DateTime.Today;
            _context.Staff.Add(newStaff);
            return newStaff;
        }

        public bool UpdateStaff(int id, Staff updatedStaff)
        {
            var existingStaff = _context.Staff.FirstOrDefault(s => s.Id == id && s.IsActive);
            if (existingStaff == null) return false;

            existingStaff.FirstName = updatedStaff.FirstName;
            existingStaff.LastName = updatedStaff.LastName;
            existingStaff.Position = updatedStaff.Position;
            existingStaff.PhoneNumber = updatedStaff.PhoneNumber;
            existingStaff.Email = updatedStaff.Email;
            existingStaff.AssignedGroupId = updatedStaff.AssignedGroupId;

            return true;
        }

        public bool DeleteStaff(int id)
        {
            var staffToRemove = _context.Staff.FirstOrDefault(s => s.Id == id);
            if (staffToRemove == null) return false;

            staffToRemove.IsActive = false; // מחיקה "רכה"
            return true;
        }

        public IEnumerable<Staff> GetStaffByPosition(string position)
        {
            return _context.Staff
                .Where(s => s.IsActive && s.Position.Equals(position, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}