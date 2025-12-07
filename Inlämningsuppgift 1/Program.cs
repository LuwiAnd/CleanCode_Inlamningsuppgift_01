var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<IC>


var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints => {  endpoints.MapControllers(); });

app.Run();

// Jag ska lägga till services här istället för i klasserna!! ??!!???
// (DI/DIP = Dependency Inversion (Principle))