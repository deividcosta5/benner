using DigitalMicrowave.Business.Repositories;
using DigitalMicrowave.Business.Services;
using DigitalMicrowave.Web.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

var jsonPath = Path.Combine(builder.Environment.ContentRootPath, "custom_programs.json");
builder.Services.AddSingleton<IProgramRepository>(_ => new JsonProgramRepository(jsonPath));
builder.Services.AddSingleton<IMicrowaveService, MicrowaveService>();
builder.Services.AddSingleton<IProgramService, ProgramService>();
builder.Services.AddHostedService<HeatingBackgroundService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.MapHub<MicrowaveHub>("/microwaveHub");

app.Run();
