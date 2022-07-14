using AutoMapper;

using Profitable.Data.Repository.Contract;
using Profitable.Data.Repository;
using Profitable.Automapper;
using Profitable.Services.Posts.Contracts;
using Profitable.Services.Posts;
using Profitable.Services.Comments.Contracts;
using Profitable.Services.Comments;
using Profitable.Models.EntityModels;
using Profitable.Services.Markets.Contract;
using Profitable.Services.Markets;
using Profitable.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection")));

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new PostsMapper());
    mc.AddProfile(new CommentsMapper());
    mc.AddProfile(new MarketsMapper());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ApplicationDbContext>();

builder.Services.AddTransient<IPostService, PostService>();
builder.Services.AddTransient<ICommentService, CommentService>();
builder.Services.AddTransient<IMarketsService, MarketsService>();

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

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseCors("policy");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
