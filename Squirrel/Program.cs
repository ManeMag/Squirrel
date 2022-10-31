using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Squirrel;
using Squirrel.Contexts;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddDbContext<ApplicationContext>(options => options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
services.AddControllers();
//services.AddControllers().AddDataAnnotationsLocalization(options =>
//{
//    options.DataAnnotationLocalizerProvider = (type, factory) =>
//        factory.Create(typeof(SharedResource));
//});
services.AddSwaggerGen();

services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});


services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en"),
        new CultureInfo("uk")
    };

    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRequestLocalization(app.Services.GetService<IOptions<RequestLocalizationOptions>>().Value);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();