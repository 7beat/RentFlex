using RentFlex.Utility.WireMock;
using RentFlex.Web.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.RegisterServices(builder.Configuration);
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("WireMockClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5000");
});

var app = builder.Build();

WireMockService.Start();
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
