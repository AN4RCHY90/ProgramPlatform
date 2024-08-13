using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProgramPlatform.Data;
using ProgramPlatform.Interfaces;
using ProgramPlatform.Models;
using ProgramPlatform.Services;
using ProgramPlatform.Utilities;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog configuration
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
var encryptionKey = builder.Configuration["EncryptionKey"];

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUserModel, ApplicationRoleModel>
        (options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Host.UseSerilog();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSingleton(new EncryptionServices(encryptionKey));
builder.Services.AddHttpClient<IZohoInterface, ZohoServices>();
builder.Services.AddScoped<IAccountInterface, AccountServices>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<IUserInterface, UserServices>();
builder.Services.AddScoped<IRoleInterface, RoleServices>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Authentication/Login";
    options.LogoutPath = "/Authentication/Logout";
});

var app = builder.Build();

// Seed roles & admin user
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUserModel>>();
    var roleManager = services.GetRequiredService<RoleManager<ApplicationRoleModel>>();
    await SeedData.Initialise(services, userManager);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();