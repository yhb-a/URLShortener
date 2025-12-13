using Microsoft.EntityFrameworkCore;
using URLShortener.Models;

namespace URLShortener.Repository
{
    public class UrlRepository(URLDbContext context) : IUrlRepository
    {
        public async Task AddAsync(Url url)
        {
            context.URLs.Add(url);
            await context.SaveChangesAsync();
        }
        
        public async Task<Url?> GetByShortCodeAsync(string shortCode)
        {
            return await context.URLs.FirstOrDefaultAsync(entity => entity.ShortCode == shortCode);
        }

        public async Task<int> GetLatestIdAsync()
        {
            var latestResult = await context.URLs.OrderByDescending(entity => entity.Id).FirstOrDefaultAsync();

            return latestResult?.Id ?? 0 ;
        }

        public async Task DeleteAsync(Url entityToDelete)
        {
            context.URLs.Remove(entityToDelete);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Url entityToUpdate)
        {
            context.URLs.Update(entityToUpdate);
            await context.SaveChangesAsync();
        }
    }
}
