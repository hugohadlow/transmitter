var builder = WebApplication.CreateBuilder(args);

//Add configuration
var configurationBuilder = new ConfigurationBuilder()
    //.SetBasePath("path here") //<--You would need to set the path
    .AddJsonFile("appsettings.json"); //or what ever file you have the settings
IConfiguration configuration = configurationBuilder.Build();
builder.Services.AddScoped<IConfiguration>(_ => configuration);

// Add services to the container.
builder.Services.AddControllers();
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
