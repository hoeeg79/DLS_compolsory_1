using CleanerService.Repository;
using CleanerService.Service;

var builder = WebApplication.CreateBuilder(args);

string hostname = "localhost:5108";
if (!builder.Environment.IsDevelopment())
{
    hostname = "db-api:8080";
}

builder.Services.AddControllers();
builder.Services.AddHttpClient<ICleanerRepository, CleanerRepository>(client =>
{
    client.BaseAddress = new Uri($"http://{hostname}");
});
builder.Services.AddScoped<ICleanerService, CleanerService.Service.CleanerService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
