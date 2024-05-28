using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Reminder.Data;
using Reminder.Models;
using Reminder.Services;
using ReminderModal.Services;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ReminderContextConnection") ?? throw new InvalidOperationException("Connection string 'ReminderContextConnection' not found.");

builder.Services.AddDbContext<ReminderContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ReminderContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHostedService<ReminderNotificationService>();
builder.Services.AddScoped<IEmailService,SmtpEmailService>();
builder.Services.AddScoped<IReminderRepository, ReminderRepository>();
builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ReminderContext>();
    context.Database.Migrate();
}

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
