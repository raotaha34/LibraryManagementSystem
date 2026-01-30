using Library_Management_System.Models;
using Library_Management_System.Services;
using Library_Management_System.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Register DbContext with SQL Server
builder.Services.AddDbContext<LibraryManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Register services
builder.Services.AddScoped<IBookServices, BookService>();
builder.Services.AddScoped<IIssueService, IssueService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IAccountService, AccountService>();

// Add distributed memory cache (required for sessions)
builder.Services.AddDistributedMemoryCache();

// Add session support
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure middleware
app.UseStaticFiles();
app.UseRouting();
app.UseSession(); // must be BEFORE UseAuthorization()
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
