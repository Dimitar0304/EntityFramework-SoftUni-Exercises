using FishingShop.Core.Contracts;
using FishingShop.Core.Services;
using FishingShop.Infrastructure.Data;
using FishingShop.Infrastructure.Data.Common;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//use dependency injection for db context data 
builder.Services.AddDbContext<FishingShopDbContext>
    (options => options.UseSqlServer
    (builder.Configuration.GetConnectionString("FishingShopConnectionString")));

//adding in service repository 
builder.Services.AddScoped<IRepository,Repository>();

//adding service dependecy
builder.Services.AddScoped<IFishingRodService, FishingRodService>();
builder.Services.AddControllers();

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

app.Run();
