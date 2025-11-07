using URLShortener.Models;

namespace URLShortener.Service
{
    public interface IURLService
    {
        Task<string> ShortenCode(string longURL);
        Task<string> GetLongUrl(string shortCode);
        Task Delete(string shortCode);
        Task Update(string shortCode, string longURL);
        void DeleteAll();
    }
}
