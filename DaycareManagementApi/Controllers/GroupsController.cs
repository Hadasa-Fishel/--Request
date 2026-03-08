using Daycare.Core.Entities;
using Daycare.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DaycareManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Group>> GetGroups()
        {
            return Ok(_groupService.GetGroups());
        }

        [HttpGet("{id}")]
        public ActionResult<Group> GetGroup(int id)
        {
            var group = _groupService.GetGroupById(id);
            if (group == null)
            {
                return NotFound($"Group with Id {id} was not found or is inactive.");
            }
            return Ok(group);
        }

        [HttpPost]
        public ActionResult<Group> AddGroup([FromBody] Group newGroup)
        {
            var addedGroup = _groupService.AddGroup(newGroup);
            return CreatedAtAction(nameof(GetGroup), new { id = addedGroup.Id }, addedGroup);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateGroup(int id, [FromBody] Group updatedGroup)
        {
            var success = _groupService.UpdateGroup(id, updatedGroup);
            if (!success)
            {
                return NotFound($"Group with Id {id} was not found or is inactive.");
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteGroup(int id)
        {
            var result = _groupService.DeleteGroup(id);

            if (!result.Success)
            {
                if (result.ErrorMessage == "NotFound")
                {
                    return NotFound($"Group with Id {id} was not found.");
                }
                // אם יש הודעת שגיאה אחרת (כמו "אסור למחוק קבוצה עם ילדים") נחזיר בקשה שגויה
                return BadRequest(result.ErrorMessage);
            }

            return NoContent();
        }

        [HttpGet("available")]
        public ActionResult<IEnumerable<Group>> GetAvailableGroups()
        {
            var availableGroups = _groupService.GetAvailableGroups();
            if (!availableGroups.Any())
            {
                return NotFound("No groups with available space.");
            }
            return Ok(availableGroups);
        }

        [HttpGet("{id}/stats")]
        public ActionResult<object> GetGroupStats(int id)
        {
            var stats = _groupService.GetGroupStats(id);
            if (stats == null)
            {
                return NotFound($"Group with Id {id} was not found or is inactive.");
            }
            return Ok(stats);
        }
    }
}