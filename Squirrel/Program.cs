using Microsoft.EntityFrameworkCore;
using Squirrel.Contexts;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddDbContext<ApplicationContext>(options => options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
services.AddControllers();
services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();