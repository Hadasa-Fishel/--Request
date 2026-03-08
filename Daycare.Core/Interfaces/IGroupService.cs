using Daycare.Core.Entities;

namespace Daycare.Core.Interfaces
{
	public interface IGroupService
	{
		IEnumerable<Group> GetGroups();
		Group GetGroupById(int id);
		Group AddGroup(Group newGroup);
		bool UpdateGroup(int id, Group updatedGroup);

		(bool Success, string ErrorMessage) DeleteGroup(int id);

		IEnumerable<Group> GetAvailableGroups();
		object GetGroupStats(int id);
	}
}