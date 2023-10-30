using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("LocalSqlServer")));

builder.Services.AddTransient<IUrlInterface, UrlRepository>();

var app = builder.Build();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=home}/{action=index}/{id?}"
);

app.Run();