using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RahtakApi.Entities.Models;
using RahtakApi.DAL.Data;

namespace RahtakApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubServicesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;

        public SubServicesController(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        // *********** GET: api/SubServices ***********
        [HttpGet]
        public IActionResult GetSubServices()
        {
            var subServices = _context.SubServices
                .Include(s => s.ServiceGroups)
                .Include(s => s.ServiceProvider)
                    .ThenInclude(p => p.ServiceProviderType)
                .ToList();

            return Ok(subServices);
        }

        // *********** GET: api/SubServices/5 ***********
        [HttpGet("{id}")]
        public IActionResult GetSubService(int id)
        {
            var subService = _context.SubServices
                .Include(s => s.ServiceGroups)
                .Include(s => s.ServiceProvider)
                    .ThenInclude(p => p.ServiceProviderType)
                .FirstOrDefault(s => s.SubServiceId == id);

            if (subService == null)
                return NotFound();

            return Ok(subService);
        }

        // *********** NEW ENDPOINT: GET: api/SubServices/service-group/{serviceGroupId} ***********
        [HttpGet("service-group/{serviceGroupId}")]
        public IActionResult GetSubServicesByServiceGroupId(int serviceGroupId)
        {
            var subServices = _context.SubServices
                .Include(s => s.ServiceGroups)
                .Include(s => s.ServiceProvider)
                    .ThenInclude(p => p.ServiceProviderType)
                .Where(s => s.ServiceGroupId == serviceGroupId)
                .ToList();

            if (subServices == null || subServices.Count == 0)
                return NotFound("No sub-services found for the given ServiceGroupId.");

            return Ok(subServices);
        }

        // *********** POST: api/SubServices ***********
        [HttpPost]
        public IActionResult CreateSubService([FromBody] SubService subService)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _unitOfWork.SubServices.Add(subService);
            _unitOfWork.Save();

            return CreatedAtAction("GetSubService", new { id = subService.SubServiceId }, subService);
        }

        // *********** PUT: api/SubServices/5 ***********
        [HttpPut("{id}")]
        public IActionResult UpdateSubService(int id, [FromBody] SubService subService)
        {
            if (id != subService.SubServiceId)
                return BadRequest();

            var existingSubService = _unitOfWork.SubServices.GetById(id);
            if (existingSubService == null)
                return NotFound();

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
                return NotFound();

            _unitOfWork.SubServices.Delete(subService);
            _unitOfWork.Save();

            return NoContent();
        }
    }
}