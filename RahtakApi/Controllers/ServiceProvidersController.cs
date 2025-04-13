using Interfaces;
using Microsoft.AspNetCore.Mvc;
using RahtakApi.Entities.Models;

namespace RahtakApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceProvidersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceProvidersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // *********** GET: api/ServiceProviders ***********
        [HttpGet]
        public IActionResult GetServiceProviders()
        {
            var serviceProviders = _unitOfWork.ServiceProviders.GetAll();
            return Ok(serviceProviders);
        }

        // *********** GET: api/ServiceProviders/5 ***********
        [HttpGet("{id}")]
        public IActionResult GetServiceProvider(int id)
        {
            var serviceProvider = _unitOfWork.ServiceProviders.GetById(id);

            if (serviceProvider == null)
            {
                return NotFound();
            }

            return Ok(serviceProvider);
        }

        // *********** POST: api/ServiceProviders ***********
        [HttpPost]
        public IActionResult CreateServiceProvider([FromBody] ServiceProviders serviceProvider)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _unitOfWork.ServiceProviders.Add(serviceProvider);
            _unitOfWork.Save();

            return CreatedAtAction("GetServiceProvider", new { id = serviceProvider.ServiceProviderId }, serviceProvider);
        }

        // *********** PUT: api/ServiceProviders/5 ***********
        [HttpPut("{id}")]
        public IActionResult UpdateServiceProvider(int id, [FromBody] ServiceProviders serviceProvider)
        {
            if (id != serviceProvider.ServiceProviderId)
            {
                return BadRequest();
            }

            var existingServiceProvider = _unitOfWork.ServiceProviders.GetById(id);
            if (existingServiceProvider == null)
            {
                return NotFound();
            }

            existingServiceProvider.Name = serviceProvider.Name;
            existingServiceProvider.Telephone = serviceProvider.Telephone;
            existingServiceProvider.Email = serviceProvider.Email;
            existingServiceProvider.Enabled = serviceProvider.Enabled;
            existingServiceProvider.ServiceProviderTypeId = serviceProvider.ServiceProviderTypeId;

            _unitOfWork.ServiceProviders.Update(existingServiceProvider);
            _unitOfWork.Save();

            return NoContent();
        }

        // *********** DELETE: api/ServiceProviders/5 ***********
        [HttpDelete("{id}")]
        public IActionResult DeleteServiceProvider(int id)
        {
            var serviceProvider = _unitOfWork.ServiceProviders.GetById(id);
            if (serviceProvider == null)
            {
                return NotFound();
            }

            _unitOfWork.ServiceProviders.Delete(serviceProvider);
            _unitOfWork.Save();

            return NoContent();
        }
    }
}
