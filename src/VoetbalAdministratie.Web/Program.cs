using Microsoft.Data.Sqlite;
using System.IO;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var nlCulture = new CultureInfo("nl-NL");
CultureInfo.DefaultThreadCurrentCulture = nlCulture;
CultureInfo.DefaultThreadCurrentUICulture = nlCulture;
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(nlCulture);
    options.SupportedCultures = new List<CultureInfo> { nlCulture };
    options.SupportedUICultures = new List<CultureInfo> { nlCulture };
});

var rawConnectionString = builder.Configuration.GetConnectionString("VoetbalDb")
    ?? throw new InvalidOperationException("Connection string 'VoetbalDb' ontbreekt.");

// SQLite-basispad is gevoelig voor de working directory bij `dotnet run`.
// Maak daarom het db-pad altijd absoluut op basis van ContentRootPath en zorg dat de map bestaat.
var connectionStringBuilder = new SqliteConnectionStringBuilder(rawConnectionString);
var dataSource = connectionStringBuilder.DataSource;
if (string.IsNullOrWhiteSpace(dataSource))
{
    throw new InvalidOperationException("Connection string 'VoetbalDb' mist een Data Source.");
}

if (!Path.IsPathRooted(dataSource))
{
    dataSource = Path.Combine(builder.Environment.ContentRootPath, dataSource);
}

var directory = Path.GetDirectoryName(dataSource);
if (string.IsNullOrWhiteSpace(directory))
{
    throw new InvalidOperationException("Kon SQLite database directory niet bepalen.");
}

Directory.CreateDirectory(directory);
connectionStringBuilder.DataSource = dataSource;

var connectionString = connectionStringBuilder.ToString();

builder.Services.AddSingleton(new VoetbalAdministratie.Web.Data.Db(connectionString));
builder.Services.AddSingleton<VoetbalAdministratie.Web.Data.DbInitializer>();
builder.Services.AddScoped<VoetbalAdministratie.Web.Repositories.LidRepository>();
builder.Services.AddScoped<VoetbalAdministratie.Web.Repositories.LidTeamRepository>();
builder.Services.AddScoped<VoetbalAdministratie.Web.Repositories.BetalingRepository>();
builder.Services.AddScoped<VoetbalAdministratie.Web.Repositories.PlanningRepository>();
builder.Services.AddScoped<VoetbalAdministratie.Web.Repositories.LookupRepository>();
builder.Services.AddScoped<VoetbalAdministratie.Web.Services.LidService>();
builder.Services.AddScoped<VoetbalAdministratie.Web.Services.BetalingService>();
builder.Services.AddScoped<VoetbalAdministratie.Web.Services.PlanningService>();

var app = builder.Build();

app.UseRequestLocalization();

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<VoetbalAdministratie.Web.Data.DbInitializer>();
    initializer.EnsureCreatedAndSeeded();
}

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

app.Run();
