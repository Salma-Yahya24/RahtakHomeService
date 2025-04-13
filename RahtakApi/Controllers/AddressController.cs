using Interfaces;
using Microsoft.AspNetCore.Mvc;
using RahtakApi.Entities.Models;


namespace RahtakApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressContollor : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddressContollor(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // *********** GET: api/Addresses ***********
        [HttpGet]
        public IActionResult GetAddresses()
        {
            var addresses = _unitOfWork.Addresses.GetAll();
            return Ok(addresses);
        }

        // *********** GET: api/Addresses/5 ***********
        [HttpGet("{id}")]
        public IActionResult GetAddress(int id)
        {
            var address = _unitOfWork.Addresses.GetById(id);

            if (address == null)
            {
                return NotFound();
            }

            return Ok(address);
        }

        // *********** POST: api/Addresses ***********
        [HttpPost]
        public IActionResult CreateAddress([FromBody] Address address)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _unitOfWork.Addresses.Add(address);
            _unitOfWork.Save();

            return CreatedAtAction("GetAddress", new { id = address.AddressId }, address);
        }

        // *********** PUT: api/Addresses/5 ***********
        [HttpPut("{id}")]
        public IActionResult UpdateAddress(int id, [FromBody] Address address)
        {
            if (id != address.AddressId)
            {
                return BadRequest();
            }

            var existingAddress = _unitOfWork.Addresses.GetById(id);
            if (existingAddress == null)
            {
                return NotFound();
            }

            existingAddress.UserId = address.UserId;
            existingAddress.ServiceProviderId = address.ServiceProviderId;
            existingAddress.Street = address.Street;
            existingAddress.City = address.City;
            existingAddress.State = address.State;
            existingAddress.ZipCode = address.ZipCode;
            existingAddress.Country = address.Country;

            _unitOfWork.Addresses.Update(existingAddress);
            _unitOfWork.Save();

            return NoContent();
        }

        // *********** DELETE: api/Addresses/5 ***********
        [HttpDelete("{id}")]
        public IActionResult DeleteAddress(int id)
        {
            var address = _unitOfWork.Addresses.GetById(id);
            if (address == null)
            {
                return NotFound();
            }

            _unitOfWork.Addresses.Delete(address);
            _unitOfWork.Save();

            return NoContent();
        }
    }
}
