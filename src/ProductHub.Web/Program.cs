using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductHub.Web.Data;

var builder = WebApplication.CreateBuilder(args);

//NEW: Read ConnectionString From 
var connectionstring = builder.Configuration.GetConnectionString("ProductHubConnectionString");
//NEW: Connect to the sqlserver
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionstring));

//NEW: Identity(login system) and tene database sathe connect karvi
builder.Services
    .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();


// Add services to the container.
builder.Services.AddControllersWithViews();

//NEW: Identity UI pages Razor Pages chhe
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

//NEW: Who are you?
app.UseAuthentication();
app.UseAuthorization();  // What is allowed for this User?
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    //pattern: "{controller=Products}/{action=Index}/{id?}")
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

//NEW: ... MapStaticAssets, MapControllerRoute same ...
app.MapRazorPages();

app.Run();
