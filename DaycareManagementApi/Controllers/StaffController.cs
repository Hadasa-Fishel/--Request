using Daycare.Core.Entities;
using Daycare.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DaycareManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;

        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Staff>> GetStaff()
        {
            return Ok(_staffService.GetStaff());
        }

        [HttpGet("{id}")]
        public ActionResult<Staff> GetStaffMember(int id)
        {
            var staffMember = _staffService.GetStaffMember(id);
            if (staffMember == null)
            {
                return NotFound($"Staff member with Id {id} was not found or is inactive.");
            }
            return Ok(staffMember);
        }

        [HttpPost]
        public ActionResult<Staff> AddStaff([FromBody] Staff newStaff)
        {
            var addedStaff = _staffService.AddStaff(newStaff);
            return CreatedAtAction(nameof(GetStaffMember), new { id = addedStaff.Id }, addedStaff);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStaff(int id, [FromBody] Staff updatedStaff)
        {
            var success = _staffService.UpdateStaff(id, updatedStaff);
            if (!success)
            {
                return NotFound($"Staff member with Id {id} was not found or is inactive.");
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStaff(int id)
        {
            var success = _staffService.DeleteStaff(id);
            if (!success)
            {
                return NotFound($"Staff member with Id {id} was not found.");
            }
            return NoContent();
        }

        [HttpGet("byposition")]
        public ActionResult<IEnumerable<Staff>> GetStaffByPosition([FromQuery] string position)
        {
            var filteredStaff = _staffService.GetStaffByPosition(position);
            if (!filteredStaff.Any())
            {
                return NotFound($"No active staff found with position: {position}");
            }
            return Ok(filteredStaff);
        }
    }
}