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
        public async Task<IActionResult> Shorten([FromBody] RequestModel request)
        {
            try
            {
                var shortCode = await this.service.ShortenCode(request.Url);
                var shortUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/{shortCode}";

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
        public async Task<IActionResult> UpdateShortCode(string shortCode, [FromBody] RequestModel request)
        {
            try
            {
                await this.service.Update(shortCode, request.Url);

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

        [HttpDelete]
        public IActionResult DeleteAll()
        {
            this.service.DeleteAll();
            return Ok();
        }
    }
}
