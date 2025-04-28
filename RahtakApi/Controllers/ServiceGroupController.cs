using Interfaces;
using Microsoft.AspNetCore.Mvc;
using RahtakApi.Entities.DTOs;
using RahtakApi.Entities.Models;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace RahtakApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceGroupController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;

        public ServiceGroupController(IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _env = env;
        }

        [HttpGet]
        public IActionResult GetServiceGroups()
        {
            var serviceGroups = _unitOfWork.ServiceGroups.GetAll();
            return Ok(serviceGroups);
        }

        [HttpGet("{id}")]
        public IActionResult GetServiceGroup(int id)
        {
            var serviceGroup = _unitOfWork.ServiceGroups.GetById(id);
            if (serviceGroup == null)
                return NotFound();

            return Ok(serviceGroup);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public IActionResult CreateServiceGroup([FromForm] ServiceGroupForm formDto)
        {
            string? imageUrl = null;

            // إذا تم تقديم رابط الصورة، يتم استخدامه
            if (!string.IsNullOrEmpty(formDto.ImageUrl))
            {
                imageUrl = formDto.ImageUrl;
            }
            else if (formDto.Image != null && formDto.Image.Length > 0)
            {
                // إذا لم يتم تقديم رابط الصورة، يتم تحميل الصورة محليًا
                if (!Directory.Exists(Path.Combine(_env.WebRootPath, "images/serviceGroup")))
                {
                    Directory.CreateDirectory(Path.Combine(_env.WebRootPath, "images/serviceGroup"));
                }

                if (formDto.Image.Length > 10 * 1024 * 1024) // 10 MB
                {
                    ModelState.AddModelError("Image", "The image size must be less than 10MB.");
                    return BadRequest(ModelState);
                }

                var extension = Path.GetExtension(formDto.Image.FileName)?.ToLowerInvariant();
                if (!new[] { ".png", ".jpg", ".jpeg" }.Contains(extension))
                {
                    ModelState.AddModelError("Image", "Unsupported file format. Only PNG and JPG are allowed.");
                    return BadRequest(ModelState);
                }

                var imageName = $"{Guid.NewGuid()}{extension}";
                var savePath = Path.Combine(_env.WebRootPath, "images/serviceGroup", imageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    formDto.Image.CopyTo(stream);
                }

                imageUrl = $"images/serviceGroup/{imageName}";
            }
            else
            {
                // إذا لم يتم تقديم كلاهما، يتم رفض الطلب
                ModelState.AddModelError("Image", "Please provide either an image or a valid image URL.");
                return BadRequest(ModelState);
            }

            var serviceGroup = new ServiceGroups
            {
                ServiceGroupName = formDto.ServiceGroupName,
                Description = formDto.Description,
                ImageUrl = imageUrl
            };

            _unitOfWork.ServiceGroups.Add(serviceGroup);
            _unitOfWork.Save();

            return CreatedAtAction("GetServiceGroup", new { id = serviceGroup.ServiceGroupId }, serviceGroup);
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public IActionResult UpdateServiceGroup(int id, [FromForm] ServiceGroupForm formDto)
        {
            var existingServiceGroup = _unitOfWork.ServiceGroups.GetById(id);
            if (existingServiceGroup == null)
                return NotFound();

            existingServiceGroup.ServiceGroupName = formDto.ServiceGroupName;
            existingServiceGroup.Description = formDto.Description;

            if (!string.IsNullOrEmpty(formDto.ImageUrl))
            {
                // إذا تم تقديم رابط الصورة، يتم استخدامه
                existingServiceGroup.ImageUrl = formDto.ImageUrl;
            }
            else if (formDto.Image != null && formDto.Image.Length > 0)
            {
                // إذا لم يتم تقديم رابط الصورة، يتم تحميل الصورة محليًا
                if (!Directory.Exists(Path.Combine(_env.WebRootPath, "images/serviceGroup")))
                {
                    Directory.CreateDirectory(Path.Combine(_env.WebRootPath, "images/serviceGroup"));
                }

                if (formDto.Image.Length > 10 * 1024 * 1024) // 10 MB
                {
                    ModelState.AddModelError("Image", "The image size must be less than 10MB.");
                    return BadRequest(ModelState);
                }

                var extension = Path.GetExtension(formDto.Image.FileName)?.ToLowerInvariant();
                if (!new[] { ".png", ".jpg", ".jpeg" }.Contains(extension))
                {
                    ModelState.AddModelError("Image", "Unsupported file format. Only PNG and JPG are allowed.");
                    return BadRequest(ModelState);
                }

                var imageName = $"{Guid.NewGuid()}{extension}";
                var savePath = Path.Combine(_env.WebRootPath, "images/serviceGroup", imageName);

                if (!string.IsNullOrEmpty(existingServiceGroup.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_env.WebRootPath, existingServiceGroup.ImageUrl.Replace("/", "\\"));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    formDto.Image.CopyTo(stream);
                }

                existingServiceGroup.ImageUrl = $"images/serviceGroup/{imageName}";
            }

            _unitOfWork.ServiceGroups.Update(existingServiceGroup);
            _unitOfWork.Save();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteServiceGroup(int id)
        {
            var serviceGroup = _unitOfWork.ServiceGroups.GetById(id);
            if (serviceGroup == null)
                return NotFound();

            _unitOfWork.ServiceGroups.Delete(serviceGroup);
            _unitOfWork.Save();

            return NoContent();
        }
    }
}