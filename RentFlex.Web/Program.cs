using CarRental.Infrastructure.Extensions;
using RentFlex.Utility.WireMock;
using RentFlex.Web.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.RegisterServices(builder.Configuration);
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

await app.SeedIdentityAsync();

WireMockService.Start();
WireMockService.ConfigureEndpoints("592fdf9f-2395-4a12-8f66-1e8b3b53b6fc", "9d1063e1-125e-45c6-bef3-d5baaa717152");

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
    pattern: "{area=Owner}/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
