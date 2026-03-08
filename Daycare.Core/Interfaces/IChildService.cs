using Daycare.Core.Entities;

namespace Daycare.Core.Interfaces
{
    public interface IChildService
    {
        IEnumerable<Child> GetChildren();
        Child GetChildById(int id);
        Child AddChild(Child newChild);
        bool UpdateChild(int id, Child updatedChild);
        bool DeleteChild(int id);
        string CheckIn(int id);
    }
}