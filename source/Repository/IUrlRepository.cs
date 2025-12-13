using URLShortener.Models;

namespace URLShortener.Repository
{
    public interface IUrlRepository
    {
        Task AddAsync(Url url);
        Task<Url?> GetByShortCodeAsync(string shortCode);
        Task<int> GetLatestIdAsync();
        Task UpdateAsync(Url url);
        Task DeleteAsync(Url entityToDelete);
    }
}
