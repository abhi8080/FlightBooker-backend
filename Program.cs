using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.AspNetCore.Cors;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => builder.WithOrigins("http://localhost:5173").AllowAnyHeader()
);});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ApplicationDbContext>(
    o => o.UseNpgsql(Env.connectionString));

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();


/*app.UseCors(options => {
    options.WithOrigins("http://localhost:5173").
    AllowAnyMethod().
    AllowAnyHeader();
});
*/

app.UseCors(); // Enable CORS middleware

app.UseAuthorization();

app.MapControllers();

app.Run();