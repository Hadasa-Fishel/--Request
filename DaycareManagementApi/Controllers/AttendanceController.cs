using DaycareManagementApi.Data;
using DaycareManagementApi.Entities;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AttendanceController : ControllerBase
{
    private readonly IDataContext _context;
    private static int _nextId = 3;

    // Constructor - מקבל IDataContext דרך Dependency Injection
    public AttendanceController(IDataContext context)
    {
        _context = context;
    }

    // 1. שליפת רשימה (GET /api/attendance)
    [HttpGet]
    public ActionResult<IEnumerable<Attendance>> GetAttendances()
    {
        return Ok(_context.Attendances);
    }

    // 2. שליפת בודד לפי מזהה (GET /api/attendance/1)
    [HttpGet("{id}")]
    public ActionResult<Attendance> GetAttendance(int id)
    {
        var attendance = _context.Attendances.FirstOrDefault(a => a.Id == id);

        if (attendance == null)
        {
            return NotFound($"Attendance record with Id {id} was not found.");
        }

        return Ok(attendance);
    }

    // 3. הוספה - רישום הגעה (POST /api/attendance)
    [HttpPost]
    public ActionResult<Attendance> AddAttendance([FromBody] Attendance newAttendance)
    {
        var existingToday = _context.Attendances.FirstOrDefault(a =>
            a.ChildId == newAttendance.ChildId &&
            a.Date.Date == newAttendance.Date.Date);

        if (existingToday != null)
        {
            return BadRequest($"Attendance for child {newAttendance.ChildId} already exists for {newAttendance.Date:dd/MM/yyyy}");
        }

        newAttendance.Id = _nextId++;
        newAttendance.IsPresent = true;
        _context.Attendances.Add(newAttendance);

        return CreatedAtAction(nameof(GetAttendance), new { id = newAttendance.Id }, newAttendance);
    }

    // 4. עדכון (PUT /api/attendance/1)
    [HttpPut("{id}")]
    public IActionResult UpdateAttendance(int id, [FromBody] Attendance updatedAttendance)
    {
        var existingAttendance = _context.Attendances.FirstOrDefault(a => a.Id == id);

        if (existingAttendance == null)
        {
            return NotFound($"Attendance record with Id {id} was not found.");
        }

        existingAttendance.CheckInTime = updatedAttendance.CheckInTime;
        existingAttendance.CheckOutTime = updatedAttendance.CheckOutTime;
        existingAttendance.Notes = updatedAttendance.Notes;
        existingAttendance.IsPresent = updatedAttendance.IsPresent;
        existingAttendance.DailyReport = updatedAttendance.DailyReport;

        return NoContent();
    }

    // 5. מחיקה (DELETE /api/attendance/1)
    [HttpDelete("{id}")]
    public IActionResult DeleteAttendance(int id)
    {
        var attendanceToRemove = _context.Attendances.FirstOrDefault(a => a.Id == id);

        if (attendanceToRemove == null)
        {
            return NotFound($"Attendance record with Id {id} was not found.");
        }

        _context.Attendances.Remove(attendanceToRemove);

        return NoContent();
    }

    // 6. פעולה נוספת: דוח נוכחות יומי (GET /api/attendance/daily/2024-12-08)
    [HttpGet("daily/{date}")]
    public ActionResult<object> GetDailyAttendance(DateTime date)
    {
        var dailyAttendances = _context.Attendances
            .Where(a => a.Date.Date == date.Date)
            .ToList();

        if (!dailyAttendances.Any())
        {
            return NotFound($"No attendance records found for {date:dd/MM/yyyy}");
        }

        var report = new
        {
            Date = date.Date,
            TotalRecords = dailyAttendances.Count,
            Present = dailyAttendances.Count(a => a.IsPresent),
            StillAtDaycare = dailyAttendances.Count(a => a.IsCurrentlyAtDaycare),
            Records = dailyAttendances
        };

        return Ok(report);
    }

    // 7. פעולה נוספת: רישום יציאה (PUT /api/attendance/1/checkout)
    [HttpPut("{id}/checkout")]
    public IActionResult CheckOut(int id)
    {
        var attendance = _context.Attendances.FirstOrDefault(a => a.Id == id);

        if (attendance == null)
        {
            return NotFound($"Attendance record with Id {id} was not found.");
        }

        if (attendance.CheckOutTime != null)
        {
            return BadRequest("Child has already been checked out.");
        }

        attendance.CheckOutTime = DateTime.Now.TimeOfDay;

        return Ok($"Child checked out at {attendance.CheckOutTime:hh\\:mm}");
    }

    // 8. פעולה נוספת: נוכחות לפי ילד (GET /api/attendance/bychild/1)
    [HttpGet("bychild/{childId}")]
    public ActionResult<IEnumerable<Attendance>> GetAttendanceByChild(int childId)
    {
        var childAttendances = _context.Attendances
            .Where(a => a.ChildId == childId)
            .OrderByDescending(a => a.Date)
            .ToList();

        if (!childAttendances.Any())
        {
            return NotFound($"No attendance records found for child {childId}");
        }

        return Ok(childAttendances);
    }
}