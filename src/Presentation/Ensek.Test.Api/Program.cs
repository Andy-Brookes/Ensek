using Ensek.Test.Application.Mapping;
using Ensek.Test.Application.Services;
using Ensek.Test.Application.Services.Interfaces;
using Ensek.Test.DataAccess.Factories;
using Ensek.Test.DataAccess.Factories.Interfaces;
using Ensek.Test.DataAccess.Repositories;
using Ensek.Test.Domain.Interfaces.Repositories;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("EnsekTestDatabase");
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddSingleton<IDbConnectionFactory>(serviceProvider => new DbConnectionFactory(connectionString));
builder.Services.AddTransient<IAccountRepository, AccountRepository>();
builder.Services.AddTransient<ICsvFileParser, CsvFileParser>();
builder.Services.AddTransient<IMeterReadingRepository, MeterReadingRepository>();
builder.Services.AddTransient<IMeterReadingService, MeterReadingService>();
builder.Services.AddTransient<IMeterReadingValidator, MeterReadingValidator>();
builder.Services.AddControllers();
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

public partial class Program { }// required in .Net 6 to facilitate integration tests using WebApplicationFactory
