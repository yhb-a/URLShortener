using Microsoft.AspNetCore.Mvc;
using URLShortener.Models;
using URLShortener.Service;

namespace URLShortener.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class URLShortenerController : ControllerBase
    {
        private readonly IURLService service;

        public URLShortenerController(IURLService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<IActionResult> ShortenLongURL([FromBody] RequestModel request)
        {
            try
            {
                var shortCode = await this.service.ShortenCode(request.Url);
                var shortUrl = $"https://localhost:7138/URLShortener/{shortCode}";

                return Created(shortUrl, new { shortUrl });
            }
            catch (Exception)
            {
                return BadRequest("Error trying to shorten code");
            }
        }

        [HttpGet("{shortCode}")]
        public async Task<IActionResult> RedirectToOriginalURL(string shortCode)
        {
            try
            {
                var result = await this.service.GetLongUrl(shortCode);

                await this.service.IncrementAccessCount(shortCode);

                return Redirect(result);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest($"Error retrieving url for shortCode: {shortCode}");
            }
        }

        [HttpDelete("{shortCode}")]
        public async Task<IActionResult>Delete(string shortCode)
        {
            try
            {
                await this.service.Delete(shortCode);

                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest($"Error deleting for shortCode: {shortCode}");
            }
        }

        [HttpPut("{shortCode}")]
        public async Task<IActionResult> UpdateLongURL(string shortCode, [FromBody] RequestModel request)
        {
            try
            {
                await this.service.UpdateLongUrl(shortCode, request.Url);

                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest($"Error updating url for shortCode: {shortCode}");
            }
        }

        [HttpGet("{shortCode}/stats")]
        public async Task<IActionResult> GetStatistics(string shortCode)
        {
            try
            {
                var accessCount = await this.service.GetAccessCount(shortCode);

                return Ok(accessCount);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest($"Error fetching access count for shortCode: {shortCode}");
            }
        }

        [HttpDelete]
        public IActionResult DeleteAll()
        {
            this.service.DeleteAll();
            return Ok();
        }
    }
}
