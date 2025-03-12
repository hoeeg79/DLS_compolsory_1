using SearchService.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
string hostname = "localhost:5108";
if (!builder.Environment.IsDevelopment())
{
    hostname = "db-api:8080";
}

builder.Services.AddControllers();
builder.Services.AddHttpClient<ISearchRepository, SearchRepository>(client =>
{
    client.BaseAddress = new Uri($"http://{hostname}");
});


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