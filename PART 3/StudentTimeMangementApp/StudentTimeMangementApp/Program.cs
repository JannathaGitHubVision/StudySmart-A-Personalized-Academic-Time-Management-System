using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentTimeMangementApp.Areas.Identity.Data;
using StudentTimeMangementApp.Data;
var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("StudentTimeMangementAppContextConnection") ?? throw new InvalidOperationException("Connection string 'StudentTimeMangementAppContextConnection' not found.");

builder.Services.AddDbContext<StudentTimeMangementAppContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddDefaultIdentity<RegistrationLoginUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<StudentTimeMangementAppContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Allows usernames to have spaces and underscores
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";
});


// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapRazorPages();

app.Run();
