using System.ComponentModel.DataAnnotations;

namespace Shortener
{
    public class UrlDTO
    {
        [Required]
        public string Url { get; set; }
    }
}
