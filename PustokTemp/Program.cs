using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PustokMVC.Business.Implementations;
using PustokMVC.Business.Interfaces;
using PustokTemp.Business.Implementations;
using PustokTemp.Business.Interfaces;
using PustokTemp.DAL;
using PustokTemp.Models;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<PustokDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("default"));
});
builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequireNonAlphanumeric = true;
    opt.Password.RequiredLength = 8;
    opt.Password.RequireUppercase = true;
    opt.Password.RequireLowercase = true;
    opt.User.RequireUniqueEmail = true;

}
)
    .AddEntityFrameworkStores<PustokDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<ISliderService, SliderService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAccService, AccService>();
builder.Services.AddScoped<IAuthService, AuthService>();


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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
     name: "areas",
     pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
   );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();