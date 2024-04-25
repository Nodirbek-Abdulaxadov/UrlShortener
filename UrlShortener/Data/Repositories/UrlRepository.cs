using Microsoft.EntityFrameworkCore;
using System.Text;
using UrlShortener.Models;

namespace UrlShortener.Data.Repositories;

public class UrlRepository : IUrlInterface
{
	private readonly AppDbContext _dbContext;

	public UrlRepository(AppDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<UrlModel> CreateLinkAsync(string link)
	{
		var urlModel = _dbContext.UrlModels
								.FirstOrDefault(x => x.OriginalUrl == link);
		if (urlModel != null)
		{
			return urlModel;
		}

		start:
		string shortUrl = GenerateRandomString();
		if (IsNotExist(shortUrl))
		{
			UrlModel model = new()
			{
				OriginalUrl = link,
				ShortUrl = "1kb.uz/" + shortUrl
			};

			_dbContext.UrlModels.Add(model);
			await _dbContext.SaveChangesAsync();
			return model;
		}
		else
		{
			goto start;
		}
	}

    public async Task<UrlModel> CreateMockLinkAsync(string link)
    {
        var urlModel = _dbContext.UrlModels
                                .FirstOrDefault(x => x.OriginalUrl == link);
        if (urlModel != null)
        {
            return urlModel;
        }

        string shortUrl = GenerateRandomString();
        UrlModel model = new()
        {
            OriginalUrl = link,
            ShortUrl = "1kb.uz/" + shortUrl
        };

        _dbContext.UrlModels.Add(model);
        await _dbContext.SaveChangesAsync();
        return model;
    }

    public Task<UrlModel?> GetByShortUrl(string link)
	{
		var model = _dbContext.UrlModels.FirstOrDefault(u => u.ShortUrl.EndsWith(link));
		return Task.FromResult(model);
	}


	private string GenerateRandomString()
	{
		const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
		Random random = new Random();
		StringBuilder sb = new StringBuilder(5);

		for (int i = 0; i < 5; i++)
		{
			int index = random.Next(characters.Length);
			sb.Append(characters[index]);
		}

		return sb.ToString();
	}

	private bool IsNotExist(string link)
		=> !_dbContext.UrlModels.Any(i => i.ShortUrl == link);
}
