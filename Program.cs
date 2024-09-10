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
using SiparisUygulamasi.Services.OrderServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddEnvironmentVariables();

builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection(nameof(MongoDBSettings)));

builder.Services.AddSingleton<MongoDBContext>(serviceProvider =>
{
    var settings = serviceProvider.GetRequiredService<IOptions<MongoDBSettings>>();
    return new MongoDBContext(settings);
});

// SeedData service
builder.Services.AddTransient<SeedData>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "SiparisUygulamasi", Version = "v1" });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")  // Allow only this origin
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();  // Optional: if you are handling credentials
        });
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IUserRepository<User>, UserRepository>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>(); // Register ProductRepository
builder.Services.AddScoped<AddressRepository>();

builder.Services.AddScoped<IOrderProcessingService, OrderProcessingService>();
builder.Services.AddScoped<Func<IOrderProcessingService>>(provider => () => provider.GetRequiredService<IOrderProcessingService>());
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<IOrderProcessingHelper, OrderProcessingHelper>();
builder.Services.AddScoped<Func<IOrderService>>(provider => () => provider.GetRequiredService<IOrderService>());

builder.Services.AddScoped(provider => new Lazy<IOrderRepository>(() => provider.GetRequiredService<IOrderRepository>()));
builder.Services.AddScoped(provider => new Lazy<IOrderService>(() => provider.GetRequiredService<IOrderService>()));
builder.Services.AddScoped(provider => new Lazy<IShoppingCartService>(() => provider.GetRequiredService<IShoppingCartService>()));

builder.Services.AddEndpointsApiExplorer();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// Use SeedData to populate the database after creation
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var seedData = services.GetRequiredService<SeedData>();
    await seedData.SeedAsync();
}
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else 
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
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

app.UseCors("AllowSpecificOrigin");

app.MapControllers();

app.Run();