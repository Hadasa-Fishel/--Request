using DaycareManagementApi.Data;
using DaycareManagementApi.Entities;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class StaffController : ControllerBase
{
    private readonly IDataContext _context;
    private static int _nextId = 12;

    // Constructor - מקבל IDataContext דרך Dependency Injection
    public StaffController(IDataContext context)
    {
        _context = context;
    }

    // 1. שליפת רשימה (GET /api/staff)
    [HttpGet]
    public ActionResult<IEnumerable<Staff>> GetStaff()
    {
        return Ok(_context.Staff.Where(s => s.IsActive));
    }

    // 2. שליפת בודד לפי מזהה (GET /api/staff/10)
    [HttpGet("{id}")]
    public ActionResult<Staff> GetStaffMember(int id)
    {
        var staffMember = _context.Staff.FirstOrDefault(s => s.Id == id && s.IsActive);

        if (staffMember == null)
        {
            return NotFound($"Staff member with Id {id} was not found or is inactive.");
        }

        return Ok(staffMember);
    }

    // 3. הוספה (POST /api/staff)
    [HttpPost]
    public ActionResult<Staff> AddStaff([FromBody] Staff newStaff)
    {
        newStaff.Id = _nextId++;
        newStaff.HireDate = DateTime.Today;
        _context.Staff.Add(newStaff);

        return CreatedAtAction(nameof(GetStaffMember), new { id = newStaff.Id }, newStaff);
    }

    // 4. עדכון (PUT /api/staff/10)
    [HttpPut("{id}")]
    public IActionResult UpdateStaff(int id, [FromBody] Staff updatedStaff)
    {
        var existingStaff = _context.Staff.FirstOrDefault(s => s.Id == id && s.IsActive);

        if (existingStaff == null)
        {
            return NotFound($"Staff member with Id {id} was not found or is inactive.");
        }

        existingStaff.FirstName = updatedStaff.FirstName;
        existingStaff.LastName = updatedStaff.LastName;
        existingStaff.Position = updatedStaff.Position;
        existingStaff.PhoneNumber = updatedStaff.PhoneNumber;
        existingStaff.Email = updatedStaff.Email;
        existingStaff.AssignedGroupId = updatedStaff.AssignedGroupId;

        return NoContent();
    }

    // 5. מחיקה (DELETE /api/staff/10)
    [HttpDelete("{id}")]
    public IActionResult DeleteStaff(int id)
    {
        var staffToRemove = _context.Staff.FirstOrDefault(s => s.Id == id);

        if (staffToRemove == null)
        {
            return NotFound($"Staff member with Id {id} was not found.");
        }

        staffToRemove.IsActive = false;

        return NoContent();
    }

    // 6. פעולה נוספת: שליפת צוות לפי תפקיד (GET /api/staff/byposition?position=גננת)
    [HttpGet("byposition")]
    public ActionResult<IEnumerable<Staff>> GetStaffByPosition([FromQuery] string position)
    {
        var filteredStaff = _context.Staff
            .Where(s => s.IsActive && s.Position.Equals(position, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (!filteredStaff.Any())
        {
            return NotFound($"No active staff found with position: {position}");
        }

        return Ok(filteredStaff);
    }
}