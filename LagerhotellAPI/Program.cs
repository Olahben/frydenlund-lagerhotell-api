using LagerhotellAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Antiforgery;
using Auth0.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

var mongoDbSettings = new MongoDbSettings();
builder.Configuration.GetSection("LagerhotellDatabase").Bind(mongoDbSettings);
builder.Services.AddSingleton(mongoDbSettings);

builder.Services.Configure<CookiePolicyOptions>(options =>
  {
      options.MinimumSameSitePolicy = SameSiteMode.None;
  });

/*builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = builder.Configuration["Auth0:Domain"];
    options.ClientId = builder.Configuration["Auth0:ClientId"];
});*/

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddSingleton<OrderService>();
builder.Services.AddScoped<StorageUnitService>();
builder.Services.AddScoped<WarehouseHotelService>();
builder.Services.AddScoped<LocationService>();
builder.Services.AddScoped<AssetService>();
builder.Services.AddHostedService<PendingOrderHandler>();
builder.Services.AddScoped<ICompanyUserService, CompanyUserService>();
builder.Services.AddScoped<AccountManagementService>();
builder.Services.AddScoped<Auth0UserService>();
builder.Services.AddLogging(configure => configure.AddConsole());

// Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", corsBuilder =>
    {
        corsBuilder.WithOrigins("https://localhost:5001")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
    });
});

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    /*options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };*/
    options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
    options.Audience = builder.Configuration["App:HostUrl"];
});

// Configure Swagger
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

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();