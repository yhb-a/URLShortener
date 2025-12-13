using URLShortener.Models;

namespace URLShortener.Service
{
    public interface IUrlService
    {
        Task<Result<string>> GetLongUrlAsync(string shortCode);
        Task<Result<int>> GetAccessCountAsync(string shortCode);
        Task<string> ShortenLongUrlAsync(string longUrl);
        Task<Result<int>> UpdateAccessCountAsync(string shortCode);
        Task<Result<string>> UpdateLongUrlAsync(string shortCode, string longUrl);
        Task<Result<string>> DeleteAsync(string shortCode);
    }
}