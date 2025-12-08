using DaycareManagementApi.Data;
using DaycareManagementApi.Entities;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ChildrenController : ControllerBase
{
    private readonly IDataContext _context;
    private static int _nextId = 3;

    // Constructor - מקבל IDataContext דרך Dependency Injection
    public ChildrenController(IDataContext context)
    {
        _context = context;
    }

    // 1. שליפת רשימה (GET /api/children)
    [HttpGet]
    public ActionResult<IEnumerable<Child>> GetChildren()
    {
        return Ok(_context.Children.Where(c => c.IsActive));
    }

    // 2. שליפת בודד לפי מזהה (GET /api/children/1)
    [HttpGet("{id}")]
    public ActionResult<Child> GetChild(int id)
    {
        var child = _context.Children.FirstOrDefault(c => c.Id == id && c.IsActive);

        if (child == null)
        {
            return NotFound($"Child with Id {id} was not found or is inactive.");
        }

        return Ok(child);
    }

    // 3. הוספה (POST /api/children)
    [HttpPost]
    public ActionResult<Child> AddChild([FromBody] Child newChild)
    {
        newChild.Id = _nextId++;
        newChild.EnrollmentDate = DateTime.Today;
        _context.Children.Add(newChild);

        // עדכון תפוסת הקבוצה
        var group = _context.Groups.FirstOrDefault(g => g.Id == newChild.GroupId);
        if (group != null)
        {
            group.CurrentCapacity++;
        }

        return CreatedAtAction(nameof(GetChild), new { id = newChild.Id }, newChild);
    }

    // 4. עדכון (PUT /api/children/1)
    [HttpPut("{id}")]
    public IActionResult UpdateChild(int id, [FromBody] Child updatedChild)
    {
        var existingChild = _context.Children.FirstOrDefault(c => c.Id == id && c.IsActive);

        if (existingChild == null)
        {
            return NotFound($"Child with Id {id} was not found or is inactive.");
        }

        // אם הקבוצה השתנתה, עדכן תפוסות
        if (existingChild.GroupId != updatedChild.GroupId)
        {
            var oldGroup = _context.Groups.FirstOrDefault(g => g.Id == existingChild.GroupId);
            var newGroup = _context.Groups.FirstOrDefault(g => g.Id == updatedChild.GroupId);

            if (oldGroup != null) oldGroup.CurrentCapacity--;
            if (newGroup != null) newGroup.CurrentCapacity++;
        }

        // עדכון הפרטים
        existingChild.FirstName = updatedChild.FirstName;
        existingChild.LastName = updatedChild.LastName;
        existingChild.DateOfBirth = updatedChild.DateOfBirth;
        existingChild.GroupId = updatedChild.GroupId;
        existingChild.ParentName = updatedChild.ParentName;
        existingChild.ParentPhone = updatedChild.ParentPhone;
        existingChild.MedicalNotes = updatedChild.MedicalNotes;

        return NoContent();
    }

    // 5. מחיקה (DELETE /api/children/1)
    [HttpDelete("{id}")]
    public IActionResult DeleteChild(int id)
    {
        var childToRemove = _context.Children.FirstOrDefault(c => c.Id == id);

        if (childToRemove == null)
        {
            return NotFound($"Child with Id {id} was not found.");
        }

        childToRemove.IsActive = false;

        // עדכון תפוסת הקבוצה
        var group = _context.Groups.FirstOrDefault(g => g.Id == childToRemove.GroupId);
        if (group != null)
        {
            group.CurrentCapacity--;
        }

        return NoContent();
    }

    // 6. פעולה נוספת: עדכון סטטוס כניסה (POST /api/children/1/checkin)
    [HttpPost("{id}/checkin")]
    public IActionResult CheckIn(int id)
    {
        var child = _context.Children.FirstOrDefault(c => c.Id == id && c.IsActive);

        if (child == null)
        {
            return NotFound();
        }

        // יצירת רישום נוכחות חדש
        var attendance = new Attendance
        {
            Id = _context.Attendances.Any() ? _context.Attendances.Max(a => a.Id) + 1 : 1,
            ChildId = child.Id,
            Date = DateTime.Today,
            CheckInTime = DateTime.Now.TimeOfDay,
            IsPresent = true
        };

        _context.Attendances.Add(attendance);

        return Ok($"Child {child.FirstName} checked in at {DateTime.Now:HH:mm:ss}");
    }
}