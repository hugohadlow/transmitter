using Transmitter.Stores;

var builder = WebApplication.CreateBuilder(args);

//Add configuration
var configurationBuilder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json");
IConfiguration configuration = configurationBuilder.Build();

builder.Services.AddScoped<IKeyStore, KeyStore>();
builder.Services.AddScoped<IMessageStore, MessageStore>();

// Add services to the container.
//builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
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
