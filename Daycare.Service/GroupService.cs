using Daycare.Core;
using Daycare.Core.Entities;
using Daycare.Core.Interfaces;

namespace Daycare.Service
{
    public class GroupService : IGroupService
    {
        private readonly IDataContext _context;
        private static int _nextId = 104;

        public GroupService(IDataContext context)
        {
            _context = context;
        }

        public IEnumerable<Group> GetGroups()
        {
            return _context.Groups.Where(g => g.IsActive);
        }

        public Group GetGroupById(int id)
        {
            return _context.Groups.FirstOrDefault(g => g.Id == id && g.IsActive);
        }

        public Group AddGroup(Group newGroup)
        {
            newGroup.Id = _nextId++;
            newGroup.CurrentCapacity = 0;
            _context.Groups.Add(newGroup);
            return newGroup;
        }

        public bool UpdateGroup(int id, Group updatedGroup)
        {
            var existingGroup = _context.Groups.FirstOrDefault(g => g.Id == id && g.IsActive);
            if (existingGroup == null) return false;

            existingGroup.Name = updatedGroup.Name;
            existingGroup.AgeRange = updatedGroup.AgeRange;
            existingGroup.MaxCapacity = updatedGroup.MaxCapacity;
            existingGroup.RoomNumber = updatedGroup.RoomNumber;
            existingGroup.MainStaffId = updatedGroup.MainStaffId;

            return true;
        }

        public (bool Success, string ErrorMessage) DeleteGroup(int id)
        {
            var groupToRemove = _context.Groups.FirstOrDefault(g => g.Id == id);

            if (groupToRemove == null)
                return (false, "NotFound"); // מסמן שלא נמצא

            // הלוגיקה העסקית: אסור למחוק קבוצה עם ילדים!
            if (groupToRemove.CurrentCapacity > 0)
            {
                return (false, $"Cannot delete group with {groupToRemove.CurrentCapacity} children. Please move children first.");
            }

            groupToRemove.IsActive = false;
            return (true, null); // הצלחה
        }

        public IEnumerable<Group> GetAvailableGroups()
        {
            return _context.Groups.Where(g => g.IsActive && g.CurrentCapacity < g.MaxCapacity).ToList();
        }

        public object GetGroupStats(int id)
        {
            var group = _context.Groups.FirstOrDefault(g => g.Id == id && g.IsActive);
            if (group == null) return null;

            return new
            {
                GroupName = group.Name,
                TotalCapacity = group.MaxCapacity,
                CurrentOccupancy = group.CurrentCapacity,
                AvailableSpots = group.AvailableSpots,
                OccupancyPercentage = group.OccupancyRate
            };
        }
    }
}