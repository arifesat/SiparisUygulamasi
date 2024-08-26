using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SiparisUygulamasi.Models;
using SiparisUygulamasi.Data;
using SiparisUygulamasi.Services;
using Microsoft.Extensions.Options;
using SiparisUygulamasi.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<ShoppingCartRepository>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ShoppingCartService>();
//builder.Services.AddScoped<ProductService>();

// JWT Authentication
var key = Encoding.ASCII.GetBytes("YourSecretKeyHere"); // Use a secure key
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();