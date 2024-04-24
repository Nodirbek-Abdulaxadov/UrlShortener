using UrlShortener.Models;

namespace UrlShortener.Data.Repositories;

public interface IUrlInterface
{
	Task<UrlModel> CreateLinkAsync(string link);
	Task<UrlModel?> GetByShortUrl(string link);
}