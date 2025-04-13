using Interfaces;
using Microsoft.AspNetCore.Mvc;
using RahtakApi.Entities.Models;

namespace RahtakApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceProviderTypeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceProviderTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // *********** GET: api/ServiceProviderTypes ***********
        [HttpGet]
        public IActionResult GetServiceProviderTypes()
        {
            var serviceProviderTypes = _unitOfWork.ServiceProviderTypes.GetAll();
            return Ok(serviceProviderTypes);
        }

        // *********** GET: api/ServiceProviderTypes/5 ***********
        [HttpGet("{id}")]
        public IActionResult GetServiceProviderType(int id)
        {
            var serviceProviderType = _unitOfWork.ServiceProviderTypes.GetById(id);

            if (serviceProviderType == null)
            {
                return NotFound();
            }

            return Ok(serviceProviderType);
        }

        // *********** POST: api/ServiceProviderTypes ***********
        [HttpPost]
        public IActionResult CreateServiceProviderType([FromBody] ServiceProviderType serviceProviderType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _unitOfWork.ServiceProviderTypes.Add(serviceProviderType);
            _unitOfWork.Save();

            return CreatedAtAction("GetServiceProviderType", new { id = serviceProviderType.ServiceProviderTypeId }, serviceProviderType);
        }

        // *********** PUT: api/ServiceProviderTypes/5 ***********
        [HttpPut("{id}")]
        public IActionResult UpdateServiceProviderType(int id, [FromBody] ServiceProviderType serviceProviderType)
        {
            if (id != serviceProviderType.ServiceProviderTypeId)
            {
                return BadRequest();
            }

            var existingServiceProviderType = _unitOfWork.ServiceProviderTypes.GetById(id);
            if (existingServiceProviderType == null)
            {
                return NotFound();
            }

            existingServiceProviderType.Type = serviceProviderType.Type;

            _unitOfWork.ServiceProviderTypes.Update(existingServiceProviderType);
            _unitOfWork.Save();

            return NoContent();
        }

        // *********** DELETE: api/ServiceProviderTypes/5 ***********
        [HttpDelete("{id}")]
        public IActionResult DeleteServiceProviderType(int id)
        {
            var serviceProviderType = _unitOfWork.ServiceProviderTypes.GetById(id);
            if (serviceProviderType == null)
            {
                return NotFound();
            }

            _unitOfWork.ServiceProviderTypes.Delete(serviceProviderType);
            _unitOfWork.Save();

            return NoContent();
        }
    }
}
