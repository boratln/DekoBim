using AspNetCoreHero.ToastNotification;
using Microsoft.Extensions.FileProviders;
using Xceed.Document.NET;
var builder = WebApplication.CreateBuilder(args);

// ToastNotification configuration
builder.Services.AddNotyf(config => {
    config.DurationInSeconds = 3;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopRight;
});
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Oturum zaman aþýmý

}); builder.Services.AddControllersWithViews();

// CORS policy configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowSpecificOrigins",
        corsBuilder =>
        {
            corsBuilder.WithOrigins("https://192.168.1.93")
                       .AllowAnyHeader()
                       .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

// Applying CORS policy
app.UseCors("MyAllowSpecificOrigins");

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Filter}/{id?}");


    app.Run();
