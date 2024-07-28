using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TodoApp.Background;
using TodoApp.Configurations;
using TodoApp.Domain;
using TodoApp.Repositories;
using TodoApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Database>(builder.Configuration.GetSection("DataBaseConnection"));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddHttpClient();

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var config = sp.GetRequiredService<IOptions<Database>>().Value;
    return new MongoClient(config.ConnectionString);
});

builder.Services.AddTransient<ITodoService, TodoService>();
builder.Services.AddSingleton<ITodoRepository<Todo>, TodoRepository>();
builder.Services.AddHostedService<Service>();

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