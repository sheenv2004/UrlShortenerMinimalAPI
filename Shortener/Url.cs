using System.ComponentModel.DataAnnotations;

namespace Shortener
{
    public class Url
    {
        [Key]
        public int Id { get; set; }
        public string shortUrl { get; set; }
        public string url { get; set; }
        public int counter { get; set; }

    }
}
