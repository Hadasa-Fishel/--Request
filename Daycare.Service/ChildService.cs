using Daycare.Core;
using Daycare.Core.Entities;
using Daycare.Core.Interfaces;

namespace Daycare.Service
{
    public class ChildService : IChildService
    {
        private readonly IDataContext _context;
        private static int _nextId = 3;

        public ChildService(IDataContext context)
        {
            _context = context;
        }

        public IEnumerable<Child> GetChildren()
        {
            return _context.Children.Where(c => c.IsActive);
        }

        public Child GetChildById(int id)
        {
            return _context.Children.FirstOrDefault(c => c.Id == id && c.IsActive);
        }

        public Child AddChild(Child newChild)
        {
            newChild.Id = _nextId++;
            newChild.EnrollmentDate = DateTime.Today;
            _context.Children.Add(newChild);

            var group = _context.Groups.FirstOrDefault(g => g.Id == newChild.GroupId);
            if (group != null)
            {
                group.CurrentCapacity++;
            }

            return newChild;
        }

        public bool UpdateChild(int id, Child updatedChild)
        {
            var existingChild = _context.Children.FirstOrDefault(c => c.Id == id && c.IsActive);
            if (existingChild == null) return false;

            if (existingChild.GroupId != updatedChild.GroupId)
            {
                var oldGroup = _context.Groups.FirstOrDefault(g => g.Id == existingChild.GroupId);
                var newGroup = _context.Groups.FirstOrDefault(g => g.Id == updatedChild.GroupId);
                if (oldGroup != null) oldGroup.CurrentCapacity--;
                if (newGroup != null) newGroup.CurrentCapacity++;
            }

            existingChild.FirstName = updatedChild.FirstName;
            existingChild.LastName = updatedChild.LastName;
            existingChild.DateOfBirth = updatedChild.DateOfBirth;
            existingChild.GroupId = updatedChild.GroupId;
            existingChild.ParentName = updatedChild.ParentName;
            existingChild.ParentPhone = updatedChild.ParentPhone;
            existingChild.MedicalNotes = updatedChild.MedicalNotes;

            return true;
        }

        public bool DeleteChild(int id)
        {
            var childToRemove = _context.Children.FirstOrDefault(c => c.Id == id);
            if (childToRemove == null) return false;

            childToRemove.IsActive = false;

            var group = _context.Groups.FirstOrDefault(g => g.Id == childToRemove.GroupId);
            if (group != null)
            {
                group.CurrentCapacity--;
            }

            return true;
        }

        public string CheckIn(int id)
        {
            var child = _context.Children.FirstOrDefault(c => c.Id == id && c.IsActive);
            if (child == null) return null;

            var attendance = new Attendance
            {
                Id = _context.Attendances.Any() ? _context.Attendances.Max(a => a.Id) + 1 : 1,
                ChildId = child.Id,
                Date = DateTime.Today,
                CheckInTime = DateTime.Now.TimeOfDay,
                IsPresent = true
            };
            _context.Attendances.Add(attendance);

            return $"Child {child.FirstName} checked in at {DateTime.Now:HH:mm:ss}";
        }
    }
}