using URLShortener.Models;
using URLShortener.Repository;
using URLShortener.Utilities;

namespace URLShortener.Service
{
    public class UrlService(
        IUrlRepository urlRepository,
        IGlobalCounterService globalCounterService) : IUrlService
    {
        public async Task<string> ShortenLongUrlAsync(string longUrl)
        {
            var shortCode = ShortCodeHelper.GetShortCode(globalCounterService.GetCurrentCount());

            var newEntity = new Url()
            {
                Id = globalCounterService.GetCurrentCount(),
                ShortCode = shortCode,
                LongUrl = longUrl,
                CreatedAt = DateTime.UtcNow,
            };

            await urlRepository.AddAsync(newEntity);

            globalCounterService.Increment();

            return shortCode;
        }

        public async Task<Result<string>> GetLongUrlAsync(string shortCode)
        {
            var result = await urlRepository.GetByShortCodeAsync(shortCode);

            if (result == null)
            {
                return Result<string>.Failure("ShortCode not found");
            }
            
            return Result<string>.Success(result.LongUrl);
            
        }
        
        public async Task<Result<string>> UpdateLongUrlAsync(string shortCode, string newlongUrl)
        {
            var existingEntity = await urlRepository.GetByShortCodeAsync(shortCode);

            if (existingEntity == null)
            {
                return Result<string>.Failure("ShortCode not found");
            }
            
            existingEntity.LongUrl = newlongUrl;
            
            await urlRepository.UpdateAsync(existingEntity);
            
            return Result<string>.Success(newlongUrl);
        }
        
        public async Task<Result<int>> UpdateAccessCountAsync(string shortCode)
        {
            var existingEntity = await urlRepository.GetByShortCodeAsync(shortCode);

            if (existingEntity == null)
            {
                return Result<int>.Failure("ShortCode not found");
            }
            
            existingEntity.AccessCount++;
            
            await urlRepository.UpdateAsync(existingEntity);
            
            return Result<int>.Success(existingEntity.AccessCount);
        }

        public async Task<Result<int>> GetAccessCountAsync(string shortCode)
        {
            var result = await urlRepository.GetByShortCodeAsync(shortCode);

            if (result == null)
            {
                return Result<int>.Failure("ShortCode not found");
            }

            return Result<int>.Success(result.AccessCount);
        }
        
        public async Task<Result<string>> DeleteAsync(string shortCode)
        {
            var existingEntity = await urlRepository.GetByShortCodeAsync(shortCode);

            if (existingEntity == null)
            {
                return Result<string>.Failure("ShortCode not found");
            }

            await urlRepository.DeleteAsync(existingEntity);
            
            return Result<string>.Success("Short code deleted successfully");
        }
    }
}
