using Microsoft.EntityFrameworkCore;
using System.Text;
using UrlShortener.Models;

namespace UrlShortener.Data.Repositories;

public class UrlRepository : IUrlInterface
{
	//private readonly AppDbContext _dbContext;

	List<UrlModel> urlModels = new List<UrlModel>();
	//public UrlRepository(AppDbContext dbContext)
 //   {
	//	_dbContext = dbContext;
	//}

    public async Task<UrlModel> CreateLinkAsync(string link)
	{
		var urlModel = urlModels//_dbContext.UrlModels
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
				ShortUrl = "https://0.1kb.uz/" + shortUrl
			};

			urlModels.Add(model);
			return model;
		}
		else
		{
			goto start;
		}
	}

	public async Task<UrlModel> GetByShortUrl(string link)
	{
		var model = urlModels
				.FirstOrDefault(u => u.ShortUrl.EndsWith(link));
		return model;
	}


	private string GenerateRandomString()
	{
		const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
		Random random = new Random();
		StringBuilder sb = new StringBuilder(4);

		for (int i = 0; i < 4; i++)
		{
			int index = random.Next(characters.Length);
			sb.Append(characters[index]);
		}

		return sb.ToString();
	}

	private bool IsNotExist(string link)
		=> !urlModels.Any(i => i.ShortUrl == link);
}