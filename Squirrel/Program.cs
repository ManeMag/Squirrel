using Microsoft.EntityFrameworkCore;
using Squirrel.Contexts;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddDbContext<ApplicationContext>(options => options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();