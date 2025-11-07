using Microsoft.EntityFrameworkCore;
using URLShortener.Models;

namespace URLShortener.Repository
{
    // Primary keys are indexed already.
    // A database index is like the index in a book.
    // Without Index -> Scan every row
    // With Index -> Jump directly to that row.
    public class URLRepository : IURLRepository
    {
        private readonly URLDbContext context;

        public URLRepository(URLDbContext context)
        {
            this.context = context;
        }

        public async Task AddURL(Url url)
        {
            this.context.URLs.Add(url);
            await this.context.SaveChangesAsync();
        }

        public void DeleteAll() 
        {
            this.context.URLs.ExecuteDelete();
        }

        public async Task<string> GetLongURL(string shortCode)
        {
            var entity = await this.context.URLs.FirstOrDefaultAsync(entity => entity.ShortCode == shortCode);

            if(entity == null)
            {
                throw new NotFoundException($"original URL not found for shortCode: {shortCode}");
            }

            return entity.LongUrl;
        }

        public async Task<int> GetLatestId()
        {
            var latestResult = await this.context.URLs.OrderByDescending(entity => entity.Id).FirstOrDefaultAsync();

            if (latestResult == null)
            {
                return 0;
            }

            return latestResult.Id;
        }

        public async Task Delete(string shortCode)
        {
            var entityToDelete = await this.context.URLs.FirstOrDefaultAsync(entity => entity.ShortCode == shortCode);

            if (entityToDelete == null)
            {
                throw new NotFoundException($"short code: {shortCode} was not found");
            }

            this.context.URLs.Remove(entityToDelete);
            await this.context.SaveChangesAsync();
        }

        public async Task Update(string shortCode, string longUrl)
        {
            var entityToUpdate = await this.context.URLs.FirstOrDefaultAsync(entity => entity.ShortCode == shortCode);

            if (entityToUpdate == null)
            {
                throw new NotFoundException($"short code: {shortCode} was not found");
            }

            entityToUpdate.LongUrl = longUrl;
            await this.context.SaveChangesAsync();
        }
    }
}
