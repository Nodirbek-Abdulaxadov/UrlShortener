using Microsoft.AspNetCore.Mvc;
using Npgsql;
using UrlShortener.Data.Repositories;
using UrlShortener.Models;

namespace UrlShortener.Controllers;

public class HomeController : Controller
{
	private readonly IUrlInterface _urlInterface;

	public HomeController(IUrlInterface urlInterface)
    {
		_urlInterface = urlInterface;
	}

	public IActionResult Index()
	{
		return View();
	}

	public async Task<IActionResult> Mock()
	{
		var sql = System.IO.File.ReadAllText("short.sql");

        NpgsqlConnection connection = new("Host=localhost;Database=UrlDB1;User ID=postgres; Password=1234;Port=5432;");
		try
		{
            connection.Open();
			NpgsqlCommand command = new(sql, connection);
			await command.ExecuteNonQueryAsync();
        }
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
        }
		finally
        {
            connection.Close();
        }

		return RedirectToAction("index");
	}

	[HttpPost]
	public async Task<IActionResult> Create(UrlModel urlModel)
	{
        var model = await _urlInterface.CreateLinkAsync(urlModel.OriginalUrl);
		return View("index", model);
	}

	[HttpGet("{shortUrl:length(5)}")]
    public async Task<IActionResult> Go(string shortUrl)
	{
		if (shortUrl.ToLower() == "home")
		{
			return RedirectToAction("index");
		}

		var model = await _urlInterface.GetByShortUrl(shortUrl);

		if (model != null)
		{
			return Redirect(model.OriginalUrl);
		}

		return NotFound();
	}
}