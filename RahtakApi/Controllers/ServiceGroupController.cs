using Interfaces;
using Microsoft.AspNetCore.Mvc;
using RahtakApi.Entities.Models;

namespace RahtakApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceGroupController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceGroupController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // *********** GET: api/ServiceGroups ***********
        [HttpGet]
        public IActionResult GetServiceGroups()
        {
            var serviceGroups = _unitOfWork.ServiceGroups.GetAll();
            return Ok(serviceGroups);
        }

        // *********** GET: api/ServiceGroups/5 ***********
        [HttpGet("{id}")]
        public IActionResult GetServiceGroup(int id)
        {
            var serviceGroup = _unitOfWork.ServiceGroups.GetById(id);

            if (serviceGroup == null)
            {
                return NotFound();
            }

            return Ok(serviceGroup);
        }

        // *********** POST: api/ServiceGroups ***********
        [HttpPost]
        public IActionResult CreateServiceGroup([FromBody] ServiceGroups serviceGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _unitOfWork.ServiceGroups.Add(serviceGroup);
            _unitOfWork.Save();

            return CreatedAtAction("GetServiceGroup", new { id = serviceGroup.ServiceGroupId }, serviceGroup);
        }

        // *********** PUT: api/ServiceGroups/5 ***********
        [HttpPut("{id}")]
        public IActionResult UpdateServiceGroup(int id, [FromBody] ServiceGroups serviceGroup)
        {
            if (id != serviceGroup.ServiceGroupId)
            {
                return BadRequest();
            }

            var existingServiceGroup = _unitOfWork.ServiceGroups.GetById(id);
            if (existingServiceGroup == null)
            {
                return NotFound();
            }

            existingServiceGroup.ServiceGroupName = serviceGroup.ServiceGroupName;
            existingServiceGroup.Description = serviceGroup.Description;

            _unitOfWork.ServiceGroups.Update(existingServiceGroup);
            _unitOfWork.Save();

            return NoContent();
        }

        // *********** DELETE: api/ServiceGroups/5 ***********
        [HttpDelete("{id}")]
        public IActionResult DeleteServiceGroup(int id)
        {
            var serviceGroup = _unitOfWork.ServiceGroups.GetById(id);
            if (serviceGroup == null)
            {
                return NotFound();
            }

            _unitOfWork.ServiceGroups.Delete(serviceGroup);
            _unitOfWork.Save();

            return NoContent();
        }
    }
}
