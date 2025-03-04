using DatabaseService.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Adding Swagger/OpenAPI
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adding Database Context
builder.Services.AddDbContext<DatabaseContext>();

// Adding Controllers and Repos
builder.Services.AddControllers();
builder.Services.AddScoped<IDatabaseRepository, DatabaseRepository>();

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