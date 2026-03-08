using Daycare.Core.Entities;
using Daycare.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DaycareManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChildrenController : ControllerBase
    {
        private readonly IChildService _childService;

        public ChildrenController(IChildService childService)
        {
            _childService = childService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Child>> GetChildren()
        {
            return Ok(_childService.GetChildren());
        }

        [HttpGet("{id}")]
        public ActionResult<Child> GetChild(int id)
        {
            var child = _childService.GetChildById(id);
            if (child == null)
            {
                return NotFound($"Child with Id {id} was not found or is inactive.");
            }
            return Ok(child);
        }

        [HttpPost]
        public ActionResult<Child> AddChild([FromBody] Child newChild)
        {
            var addedChild = _childService.AddChild(newChild);
            return CreatedAtAction(nameof(GetChild), new { id = addedChild.Id }, addedChild);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateChild(int id, [FromBody] Child updatedChild)
        {
            var success = _childService.UpdateChild(id, updatedChild);
            if (!success)
            {
                return NotFound($"Child with Id {id} was not found or is inactive.");
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteChild(int id)
        {
            var success = _childService.DeleteChild(id);
            if (!success)
            {
                return NotFound($"Child with Id {id} was not found.");
            }
            return NoContent();
        }

        [HttpPost("{id}/checkin")]
        public IActionResult CheckIn(int id)
        {
            var result = _childService.CheckIn(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}