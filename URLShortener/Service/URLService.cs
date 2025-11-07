using URLShortener.Models;
using URLShortener.Repository;

namespace URLShortener.Service
{
    public class URLService : IURLService
    {
        IURLRepository uRLRepository;
        IGlobalCounter globalCounter; 

        public URLService(
            IURLRepository uRLRepository, 
            IGlobalCounter globalCounter)
        {
            this.uRLRepository = uRLRepository;
            this.globalCounter = globalCounter;
        }

        public async Task<string> ShortenCode(string longURL)
        {
            var shortCode = ShortCodeHelper.GetShortCode(this.globalCounter.GetCurrentCount());

            var mapping = new Url()
            {
                Id = this.globalCounter.GetCurrentCount(),
                ShortCode = shortCode,
                LongUrl = longURL,
                CreatedAt = DateTime.UtcNow,
            };

            await this.uRLRepository.AddURL(mapping);

            this.globalCounter.Increment();

            return shortCode;
        }

        public async Task<string> GetLongUrl(string shortCode)
        {
            return await this.uRLRepository.GetLongURL(shortCode);
        }
        
        public async Task Delete(string shortCode)
        {
            await this.uRLRepository.Delete(shortCode);
        }

        public async Task Update(string shortCode, string longURL)
        {
            await this.uRLRepository.Update(shortCode, longURL);
        }

        public void DeleteAll()
        {
            this.uRLRepository.DeleteAll();
        }
    }
}
