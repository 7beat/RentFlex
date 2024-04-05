using CarRental.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using RentFlex.Infrastructure.Data;
using RentFlex.Web.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.RegisterServices(builder.Configuration);
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();

    using var scope = app.Services.CreateScope();
    var scopedServices = scope.ServiceProvider;
    var dbContext = scopedServices.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();
    await app.SeedIdentityAsync();
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
    pattern: "{area=Owner}/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
