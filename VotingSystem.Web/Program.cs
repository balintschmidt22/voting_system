using System.Globalization;
using Microsoft.AspNetCore.Localization;
using VotingSystem.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { new CultureInfo("hu-HU"), new CultureInfo("en-US") };
    options.DefaultRequestCulture = new RequestCulture("hu-HU");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();


builder.Services.AddDataAccess(builder.Configuration);
// builder.Services.AddWebAutomapper();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<VotingSystemDbContext>();
    DbInitializer.Initialize(context);
}

app.Run();