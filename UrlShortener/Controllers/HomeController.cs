using Microsoft.AspNetCore.Mvc;
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

	[HttpPost]
	public async Task<IActionResult> Create(UrlModel urlModel)
	{
        var model = await _urlInterface.CreateLinkAsync(urlModel.OriginalUrl);
		return View("index", model);
	}

	[HttpGet("{shortUrl:length(4)}")]
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