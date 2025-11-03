using Microsoft.AspNetCore.Http.Features;
using VideoApp.Web.Application.Abstractions;
using VideoApp.Web.Infrastructure.Configuration;
using VideoApp.Web.Infrastructure.FileSystem;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MediaOptions>(
    builder.Configuration.GetSection("Media"));

builder.WebHost.ConfigureKestrel(o => o.Limits.MaxRequestBodySize = null);

builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartHeadersLengthLimit = int.MaxValue;
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IVideoStorage, PhysicalVideoStorage>();
builder.Services.AddScoped<IVideoCatalogue, PhysicalVideoCatalogue>();

WebApplication app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
