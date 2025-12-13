using Microsoft.AspNetCore.Mvc;
using URLShortener.Models;
using URLShortener.Service;

namespace URLShortener.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UrlShortenerController(IUrlService service) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> ShortenLongUrl([FromBody] ShortenLongUrlRequest shortenLongUrlRequest)
        {
            try
            {
                var shortCode = await service.ShortenLongUrlAsync(shortenLongUrlRequest.Url);
                var shortenedUrl = $"https://localhost:7138/URLShortener/{shortCode}";
                
                return Ok(shortenedUrl);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error trying to shorten code for longUrl:" +
                                  $" {shortenLongUrlRequest.Url} with exception: " + ex);
            }
        }

        [HttpGet("{shortCode}")]
        public async Task<IActionResult> RedirectToOriginalUrl(string shortCode)
        {
            try
            {
                var longUrlResult = await service.GetLongUrlAsync(shortCode);

                if (!longUrlResult.IsSuccessful)
                {
                    return NotFound(longUrlResult.ErrorMessage);
                }
                
                await service.UpdateAccessCountAsync(shortCode);
                
                return Redirect(longUrlResult.Data);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving url for shortCode: { shortCode } with exception: {ex}");
            }
        }
        
        [HttpPut("{shortCode}")]
        public async Task<IActionResult> UpdateLongUrl(string shortCode, [FromBody] ShortenLongUrlRequest shortenLongUrlRequest)
        {
            try
            {
                var result = await service.UpdateLongUrlAsync(shortCode, shortenLongUrlRequest.Url);

                if (!result.IsSuccessful)
                {
                    return NotFound(result.ErrorMessage);
                }
                
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating url for shortCode: {shortCode}  with exception: {ex}");
            }
        }

        [HttpGet("{shortCode}/stats")]
        public async Task<IActionResult> GetStatistics(string shortCode)
        {
            try
            {
                var result = await service.GetAccessCountAsync(shortCode);

                if (!result.IsSuccessful)
                {
                    return NotFound(result.ErrorMessage);
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching access count for shortCode: {shortCode}  with exception: {ex}");
            }
        }
        
        [HttpDelete("{shortCode}")]
        public async Task<IActionResult> Delete(string shortCode)
        {
            try
            {
                var result = await service.DeleteAsync(shortCode);

                if (!result.IsSuccessful)
                {
                    return NotFound(result.ErrorMessage);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting for shortCode: {shortCode} with exception: {ex} ");
            }
        }
    }
}
