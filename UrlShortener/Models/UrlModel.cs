using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models;

public class UrlModel
{
    [Key, Required]
    public int Id { get; set; }
    [Required]
    public string OriginalUrl { get; set; } = string.Empty;
    public string ShortUrl { get; set; } = string.Empty;
}