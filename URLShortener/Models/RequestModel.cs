using System.ComponentModel.DataAnnotations;

namespace URLShortener.Models
{
    public class RequestModel
    {
        // Attribute provides URL validation
        [Url(ErrorMessage = "The value provided is not a valid URL.")]
        public string Url { get; set; }
    }
}
