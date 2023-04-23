using Microsoft.EntityFrameworkCore;
using backend.Data;



 DotNetEnv.Env.Load();
    


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        builder => builder.WithOrigins("http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});



string connectionString = Environment.GetEnvironmentVariable("ConnectionString");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ApplicationDbContext>(
    o => o.UseNpgsql(connectionString));

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowOrigin"); // Enable CORS middleware

app.UseAuthorization();

app.MapControllers();

app.Run();
