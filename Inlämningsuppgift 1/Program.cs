using Inlämningsuppgift_1.Repository.Interfaces;
using Inlämningsuppgift_1.Repository.Implementations;
using Inlämningsuppgift_1.Services.Interfaces;
using Inlämningsuppgift_1.Services.Implementations;

//using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


// Om jag hade använt en databas och EF Core, så hade jag inte använt
// AddSingleton här, utan AddScoped för att det används till EF Core.
// Nu vill jag dock hålla "data" i minnet under hela programmets 
// körning och då är AddSingleton mer logiskt.
builder.Services.AddSingleton<ICartRepository, CartRepository>();
builder.Services.AddSingleton<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();

builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//app.UseRouting();
//app.UseEndpoints(endpoints => {  endpoints.MapControllers(); });

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

// Jag ska lägga till services här istället för i klasserna!! ??!!???
// (DI/DIP = Dependency Inversion (Principle))