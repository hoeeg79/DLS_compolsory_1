using DatabaseService.Repository;
using DatabaseService.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Adding Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string hostname = "localhost";
if (!builder.Environment.IsDevelopment())
{
    hostname = "db-postgres";
}
string username = Environment.GetEnvironmentVariable("POSTGRES_USER");
string password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");

// Adding Database Context
builder.Services.AddDbContext<DatabaseContext>(option =>
    option.UseNpgsql(
        $"Host={hostname};Port=5432;Database=postgres;Username={username};Password={password};Trust Server Certificate=true;"));

// Adding Controllers and Repos
builder.Services.AddScoped<IDatabaseRepository, DatabaseRepository>();
builder.Services.AddScoped<IDatabaseService, DatabaseService.Service.DatabaseService>();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();