using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SiparisUygulamasi.Models;
using SiparisUygulamasi.Data;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection(nameof(MongoDBSettings)));

builder.Services.AddSingleton<MongoDBContext>(serviceProvider =>
{
    var settings = serviceProvider.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    return new MongoDBContext(settings.ConnectionString, settings.DatabaseName);
});

// Add controllers
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "SiparisUygulamasi", Version = "v1" });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.UseStaticFiles(); // Ensure this is added if you are serving static files

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SiparisUygulamasi v1");
    c.RoutePrefix = string.Empty;
});

app.UseAuthorization();

app.MapControllers();

app.Run();