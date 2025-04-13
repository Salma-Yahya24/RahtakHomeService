using Interfaces;
using Microsoft.AspNetCore.Mvc;
using RahtakApi.Entities.Models;

namespace RahtakApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubServicesControllor : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubServicesControllor(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // *********** GET: api/SubServices ***********
        [HttpGet]
        public IActionResult GetSubServices()
        {
            var subServices = _unitOfWork.SubServices.GetAll();
            return Ok(subServices);
        }

        // *********** GET: api/SubServices/5 ***********
        [HttpGet("{id}")]
        public IActionResult GetSubService(int id)
        {
            var subService = _unitOfWork.SubServices.GetById(id);

            if (subService == null)
            {
                return NotFound();
            }

            return Ok(subService);
        }

        // *********** POST: api/SubServices ***********
        [HttpPost]
        public IActionResult CreateSubService([FromBody] SubService subService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _unitOfWork.SubServices.Add(subService);
            _unitOfWork.Save();

            return CreatedAtAction("GetSubService", new { id = subService.SubServiceId }, subService);
        }

        // *********** PUT: api/SubServices/5 ***********
        [HttpPut("{id}")]
        public IActionResult UpdateSubService(int id, [FromBody] SubService subService)
        {
            if (id != subService.SubServiceId)
            {
                return BadRequest();
            }

            var existingSubService = _unitOfWork.SubServices.GetById(id);
            if (existingSubService == null)
            {
                return NotFound();
            }

            existingSubService.SubServiceName = subService.SubServiceName;
            existingSubService.Description = subService.Description;
            existingSubService.Price = subService.Price;
            existingSubService.Enabled = subService.Enabled;
            existingSubService.ServiceGroupId = subService.ServiceGroupId;
            existingSubService.ServiceProviderId = subService.ServiceProviderId;

            _unitOfWork.SubServices.Update(existingSubService);
            _unitOfWork.Save();

            return NoContent();
        }

        // *********** DELETE: api/SubServices/5 ***********
        [HttpDelete("{id}")]
        public IActionResult DeleteSubService(int id)
        {
            var subService = _unitOfWork.SubServices.GetById(id);
            if (subService == null)
            {
                return NotFound();
            }

            _unitOfWork.SubServices.Delete(subService);
            _unitOfWork.Save();

            return NoContent();
        }
    }
}
