using AutoMapper;
using DataAccess.Contexts;
using DataAccess.Entities;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Squirrel.Mapping;
using Squirrel.Requests.Transaction;
using Squirrel.Services;
using Squirrel.Services.Abstractions;
using Squirrel.Services.Repositories;
using Squirrel.Services.Repositories.Abstractions;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// TODO: Carry this code to extension methods

services.AddTransient<IEmailSender, EmailSender>();

services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

services.AddScoped<BaseCategorySeeder>();

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

services.AddSingleton(mapperConfig.CreateMapper() as IMapper);

services.AddScoped<ICategoryRepository, CategoryRepository>();
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IUnitOfWork, UnitOfWork>();
services.AddScoped<IStatisticService, StatisticService>();

services.AddAuthentication()
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
    {
        options.ClientId = configuration["Authentication:Google:ClientId"];
        options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
        options.SaveTokens = true;
        options.SignInScheme = IdentityConstants.ExternalScheme;
    }); ;

services.AddDbContext<ApplicationContext>(options =>
{
    options.UseSqlite(configuration.GetConnectionString("DefaultConnection"),
        options =>
        {
            options.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName);
        });
});

services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.SignIn.RequireConfirmedEmail = true;
}).AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

services.AddLocalization(options => { options.ResourcesPath = "Resources"; });


services
    .AddControllers()
    .AddFluentValidation(opt =>
    {
        opt.RegisterValidatorsFromAssemblyContaining<CreateTransactionRequest>();
    });

services.AddCors();
services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = 403;
        return Task.CompletedTask;
    };
});
services.AddSwaggerGen();


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
    app.UseDeveloperExceptionPage();
}
app.UseCors(builder => builder
    .WithOrigins("http://178.216.32.63",
                 "https://178.216.32.63",
                 "http://geneirodan.zapto.org",
                 "https://geneirodan.zapto.org",
                 "https://play.google.com",
                 "https://accounts.google.com",
                 "google.com")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials());

app.UseCookiePolicy(new CookiePolicyOptions() { MinimumSameSitePolicy = SameSiteMode.Lax });
var supportedCultures = new[]
{
    new CultureInfo("en"),
    new CultureInfo("uk"),
};
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();