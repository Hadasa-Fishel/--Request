using Daycare.Core.Entities;
using Daycare.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Daycare.Service;

namespace DaycareManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Attendance>> GetAttendances()
        {
            return Ok(_attendanceService.GetAttendances());
        }

        [HttpGet("{id}")]
        public ActionResult<Attendance> GetAttendance(int id)
        {
            var attendance = _attendanceService.GetAttendance(id);
            if (attendance == null)
            {
                return NotFound($"Attendance record with Id {id} was not found.");
            }
            return Ok(attendance);
        }

        [HttpPost]
        public ActionResult<Attendance> AddAttendance([FromBody] Attendance newAttendance)
        {
            var result = _attendanceService.AddAttendance(newAttendance);

            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return CreatedAtAction(nameof(GetAttendance), new { id = result.Data.Id }, result.Data);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAttendance(int id, [FromBody] Attendance updatedAttendance)
        {
            var success = _attendanceService.UpdateAttendance(id, updatedAttendance);
            if (!success)
            {
                return NotFound($"Attendance record with Id {id} was not found.");
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAttendance(int id)
        {
            var success = _attendanceService.DeleteAttendance(id);
            if (!success)
            {
                return NotFound($"Attendance record with Id {id} was not found.");
            }
            return NoContent();
        }

        [HttpGet("daily/{date}")]
        public ActionResult<object> GetDailyAttendance(DateTime date)
        {
            var report = _attendanceService.GetDailyAttendance(date);
            if (report == null)
            {
                return NotFound($"No attendance records found for {date:dd/MM/yyyy}");
            }
            return Ok(report);
        }

        [HttpPut("{id}/checkout")]
        public IActionResult CheckOut(int id)
        {
            var result = _attendanceService.CheckOut(id);

            if (!result.Success)
            {
                if (result.ErrorMessage == "NotFound") return NotFound($"Attendance record with Id {id} was not found.");

                return BadRequest(result.ErrorMessage);
            }

            return Ok($"Child checked out at {result.CheckOutTime:hh\\:mm}");
        }

        [HttpGet("bychild/{childId}")]
        public ActionResult<IEnumerable<Attendance>> GetAttendanceByChild(int childId)
        {
            var childAttendances = _attendanceService.GetAttendanceByChild(childId);
            if (!childAttendances.Any())
            {
                return NotFound($"No attendance records found for child {childId}");
            }
            return Ok(childAttendances);
        }
    }
}