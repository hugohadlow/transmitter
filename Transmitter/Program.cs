using System.Text.Json.Serialization;
using Transmitter.Models;
using Transmitter.Stores;

var builder = WebApplication.CreateBuilder(args);

//Add configuration
var configurationBuilder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json");
IConfiguration configuration = configurationBuilder.Build();

builder.Services.AddScoped<IKeyStore<SigningKey>, KeyStore<SigningKey>> ();
builder.Services.AddScoped<IMessageStore, MessageStore>();

// Add services to the container.
//builder.Services.AddControllers();
builder.Services.AddControllersWithViews().AddJsonOptions(x => { x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
