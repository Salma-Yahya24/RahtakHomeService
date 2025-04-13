using Interfaces;
using Microsoft.AspNetCore.Mvc;
using RahtakApi.Entities.Models;

namespace RahtakApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // *********** GET: api/Reviews ***********
        [HttpGet]
        public IActionResult GetReviews()
        {
            var reviews = _unitOfWork.Reviews.GetAll();
            return Ok(reviews);
        }

        // *********** GET: api/Reviews/5 ***********
        [HttpGet("{id}")]
        public IActionResult GetReview(int id)
        {
            var review = _unitOfWork.Reviews.GetById(id);

            if (review == null)
            {
                return NotFound();
            }

            return Ok(review);
        }

        // *********** POST: api/Reviews ***********
        [HttpPost]
        public IActionResult CreateReview([FromBody] Reviews review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _unitOfWork.Reviews.Add(review);
            _unitOfWork.Save();

            return CreatedAtAction("GetReview", new { id = review.ReviewId }, review);
        }

        // *********** PUT: api/Reviews/5 ***********
        [HttpPut("{id}")]
        public IActionResult UpdateReview(int id, [FromBody] Reviews review)
        {
            if (id != review.ReviewId)
            {
                return BadRequest();
            }

            var existingReview = _unitOfWork.Reviews.GetById(id);
            if (existingReview == null)
            {
                return NotFound();
            }

            existingReview.UserId = review.UserId;
            existingReview.ServiceProviderId = review.ServiceProviderId;
            existingReview.Rating = review.Rating;
            existingReview.Comment = review.Comment;

            _unitOfWork.Reviews.Update(existingReview);
            _unitOfWork.Save();

            return NoContent();
        }

        // *********** DELETE: api/Reviews/5 ***********
        [HttpDelete("{id}")]
        public IActionResult DeleteReview(int id)
        {
            var review = _unitOfWork.Reviews.GetById(id);
            if (review == null)
            {
                return NotFound();
            }

            _unitOfWork.Reviews.Delete(review);
            _unitOfWork.Save();

            return NoContent();
        }
    }
}
