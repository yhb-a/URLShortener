using URLShortener.Models;

namespace URLShortener.Repository
{
    public interface IURLRepository
    {
        Task AddURL(Url url);
        Task<string> GetLongURL(string shortCode);
        Task<int> GetLatestId();
        void DeleteAll();
        Task Delete(string shortCode);
        Task Update(string shortCode, string longUrl);
    }
}
