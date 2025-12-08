using DaycareManagementApi.Data;
using DaycareManagementApi.Entities;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class GroupsController : ControllerBase
{
    private readonly IDataContext _context;
    private static int _nextId = 104;

    // Constructor - מקבל IDataContext דרך Dependency Injection
    public GroupsController(IDataContext context)
    {
        _context = context;
    }

    // 1. שליפת רשימה (GET /api/groups)
    [HttpGet]
    public ActionResult<IEnumerable<Group>> GetGroups()
    {
        return Ok(_context.Groups.Where(g => g.IsActive));
    }

    // 2. שליפת בודד לפי מזהה (GET /api/groups/101)
    [HttpGet("{id}")]
    public ActionResult<Group> GetGroup(int id)
    {
        var group = _context.Groups.FirstOrDefault(g => g.Id == id && g.IsActive);

        if (group == null)
        {
            return NotFound($"Group with Id {id} was not found or is inactive.");
        }

        return Ok(group);
    }

    // 3. הוספה (POST /api/groups)
    [HttpPost]
    public ActionResult<Group> AddGroup([FromBody] Group newGroup)
    {
        newGroup.Id = _nextId++;
        newGroup.CurrentCapacity = 0;
        _context.Groups.Add(newGroup);

        return CreatedAtAction(nameof(GetGroup), new { id = newGroup.Id }, newGroup);
    }

    // 4. עדכון (PUT /api/groups/101)
    [HttpPut("{id}")]
    public IActionResult UpdateGroup(int id, [FromBody] Group updatedGroup)
    {
        var existingGroup = _context.Groups.FirstOrDefault(g => g.Id == id && g.IsActive);

        if (existingGroup == null)
        {
            return NotFound($"Group with Id {id} was not found or is inactive.");
        }

        existingGroup.Name = updatedGroup.Name;
        existingGroup.AgeRange = updatedGroup.AgeRange;
        existingGroup.MaxCapacity = updatedGroup.MaxCapacity;
        existingGroup.RoomNumber = updatedGroup.RoomNumber;
        existingGroup.MainStaffId = updatedGroup.MainStaffId;

        return NoContent();
    }

    // 5. מחיקה (DELETE /api/groups/101)
    [HttpDelete("{id}")]
    public IActionResult DeleteGroup(int id)
    {
        var groupToRemove = _context.Groups.FirstOrDefault(g => g.Id == id);

        if (groupToRemove == null)
        {
            return NotFound($"Group with Id {id} was not found.");
        }

        if (groupToRemove.CurrentCapacity > 0)
        {
            return BadRequest($"Cannot delete group with {groupToRemove.CurrentCapacity} children. Please move children first.");
        }

        groupToRemove.IsActive = false;

        return NoContent();
    }

    // 6. פעולה נוספת: קבוצות עם מקום פנוי (GET /api/groups/available)
    [HttpGet("available")]
    public ActionResult<IEnumerable<Group>> GetAvailableGroups()
    {
        var availableGroups = _context.Groups
            .Where(g => g.IsActive && g.CurrentCapacity < g.MaxCapacity)
            .ToList();

        if (!availableGroups.Any())
        {
            return NotFound("No groups with available space.");
        }

        return Ok(availableGroups);
    }

    // 7. פעולה נוספת: סטטיסטיקות קבוצה (GET /api/groups/101/stats)
    [HttpGet("{id}/stats")]
    public ActionResult<object> GetGroupStats(int id)
    {
        var group = _context.Groups.FirstOrDefault(g => g.Id == id && g.IsActive);

        if (group == null)
        {
            return NotFound($"Group with Id {id} was not found or is inactive.");
        }

        var stats = new
        {
            GroupName = group.Name,
            TotalCapacity = group.MaxCapacity,
            CurrentOccupancy = group.CurrentCapacity,
            AvailableSpots = group.AvailableSpots,
            OccupancyPercentage = group.OccupancyRate
        };

        return Ok(stats);
    }
}