using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Profitable.Automapper;
using Profitable.Data;
using Profitable.Data.Repository;
using Profitable.Data.Repository.Contract;
using Profitable.Data.Seeding;
using Profitable.Web.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity();

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new PostsMapper());
    mc.AddProfile(new LikesMapper());
    mc.AddProfile(new CommentsMapper());
    mc.AddProfile(new MarketsMapper());
    mc.AddProfile(new UsersMapper());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddJwtAuthentication(builder.Configuration["JWT_KEY"]);
builder.Services.AddBusinessServices();

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

    new ApplicationDbContextSeeder(app.Environment.IsProduction())
        .SeedAsync(dbContext, serviceScope.ServiceProvider)
        .GetAwaiter()
        .GetResult();
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
