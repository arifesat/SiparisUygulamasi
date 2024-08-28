using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SiparisUygulamasi.Models;
using SiparisUygulamasi.Data;
using SiparisUygulamasi.Services;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using SiparisUygulamasi.Repositories;
using SiparisUygulamasi.Services.AuthServices.IndetityServices;
using SiparisUygulamasi.Services.AuthServices.LoginServices;
using SiparisUygulamasi.Services.AuthServices.TokenServices;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddEnvironmentVariables();

builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection(nameof(MongoDBSettings)));

builder.Services.AddSingleton<MongoDBContext>(serviceProvider =>
{
    var settings = serviceProvider.GetRequiredService<IOptions<MongoDBSettings>>();
    return new MongoDBContext(settings);
});

// SeedData servisini ekle
builder.Services.AddTransient<SeedData>();
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "SiparisUygulamasi", Version = "v1" });
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IUserRepository<User>, UserRepository>();

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<ShoppingCartRepository>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ShoppingCartService>();

var app = builder.Build();

// Veritabanı oluşturulduktan sonra veri eklemek için SeedData sınıfını kullan
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var seedData = services.GetRequiredService<SeedData>();
    await seedData.SeedAsync();
}

app.UseDeveloperExceptionPage();

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