using Microsoft.EntityFrameworkCore;
using Profitable.Data;
using Profitable.Data.Repository;
using Profitable.Data.Repository.Contract;
using Profitable.Web.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.ConfigureAutomapper();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddJwtAuthentication(builder.Configuration["JWT_KEY"]);
builder.Services.AddBusinessLayerServices();

builder.Services.AddCors(options =>
{
	options.AddPolicy(name: "policy",
					  policy =>
					  {
						  policy.WithOrigins("https://localhost:44415")
						  .AllowAnyHeader()
						  .AllowAnyMethod();
					  });
});

builder.Services.AddControllers();


var app = builder.Build();


using (var serviceScope = app.Services.CreateScope())
{
	var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

	dbContext.Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
	app.UseHsts();
}

app.UseCors("policy");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
