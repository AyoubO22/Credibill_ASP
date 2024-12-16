using System;
using CB_Web.Services;
using Credibill_ASP.Data;
using CrediBill_ASP.Data;
using Credibill_ASP.Models;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NETCore.MailKit.Infrastructure.Internal;

var builder = WebApplication.CreateBuilder(args);

// Configureer services (zoals AppDbContext en Identity)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Default Identity only once
builder.Services.AddDefaultIdentity<CredibillUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

//views language identification ask suffix
builder.Services.AddLocalization(options => options.ResourcesPath = "Ressources");
builder.Services.AddMvc()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IEmailSender, MailKitEmailSender>();
builder.Services.Configure<MailKitOptions>
(
    options =>
    {
        //Option 1:  Rubbish, as this information is hardcoded
        //options.Server = "ServierName";
        //options.Port = Convert.ToInt32("465");
        //options.Account ="MyAccount";
        //options.Password = "Abc!12345";
        //options.SenderEmail = "Admin@GroupBudget.be";
        //options.SenderName = "Administrator";

        // Option 2:  Dangerous, as information is available for anyone having access to appsettings.json
        //options.Server = builder.Configuration["ExternalProviders:MailKit:SMTP:Address"];
        //options.Port = Convert.ToInt32(builder.Configuration["ExternalProviders:MailKit:SMTP:Port"]);
        //options.Account = builder.Configuration["ExternalProviders:MailKit:SMTP:Account"];
        //options.Password = builder.Configuration["ExternalProviders:MailKit:SMTP:Password"];
        //options.SenderEmail = builder.Configuration["ExternalProviders:MailKit:SMTP:SenderEmail"];
        //options.SenderName = builder.Configuration["ExternalProviders:MailKit:SMTP:SenderName"];
    }
);

// Voeg de nodige services toe voor de API en Swagger
builder.Services.AddControllers(); // Voor API-functionaliteit
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CrediBill API", Version = "v1" });
});

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

var app = builder.Build();
Globals.App = app;

// Database seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    _ = SeedDataContext.Initialize(services);
}

// Middleware configureren
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CrediBill API v1"));
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

var supportedCultures = new[] { "en-US", "fr", "nl" };
var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
app.UseRequestLocalization(localizationOptions);

// Configureer de normale Razor Pages en MVC routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Voeg de routeconfiguratie toe voor de API controllers
app.MapControllers(); // Dit zorgt ervoor dat je API controllers correct worden geladen

app.Run();