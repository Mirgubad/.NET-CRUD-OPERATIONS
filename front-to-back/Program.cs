
using front_to_back.DAL;
using front_to_back.Helpers;
using Microsoft.EntityFrameworkCore;



#region Builder

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IFileService,FileService>();
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(connectionString));

#endregion

var app = builder.Build();
app.MapControllerRoute(
    name: "area",
    pattern: "{area:exists}/{controller=dashboard}/{action=index}/{id?}");
app.MapControllerRoute(
    name:"default",
    pattern: "{controller=home}/{action=index}/{id?}"
    );
app.UseStaticFiles();
app.Run();
