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
    var settings = serviceProvider.GetRequiredService<IOptions<MongoDBSettings>>();

    //var settings = serviceProvider.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    //return new MongoDBContext(settings.ConnectionString, settings.DatabaseName);
    return new MongoDBContext(settings);

});

// SeedData servisini ekle
builder.Services.AddTransient<SeedData>();
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "SiparisUygulamasi", Version = "v1" });
});

builder.Services.AddScoped<ProductRepository>();

var app = builder.Build();

// Veritabanı oluşturulduktan sonra veri eklemek için SeedData sınıfını kullan
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var seedData = services.GetRequiredService<SeedData>();
    await seedData.SeedAsync();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SiparisUygulamasi v1");
    c.RoutePrefix = string.Empty;
});

app.UseAuthorization();

app.MapControllers();

app.Run();