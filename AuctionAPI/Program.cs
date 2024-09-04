using AuctionAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using static AuctionAPI.Models.AuctionServices;
using static AuctionAPI.Repositories.repo;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Define the secret key, issuer, and audience
const string secretKey = "AuctionAPITESTSecretKeyAuctionAPITESTSecretKeyAuctionAPITESTSecretKey";
const string issuer = "yourIssuer";
const string audience = "yourAudience";

// Add services to the container.
builder.Services.AddDbContext<AuctionContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<IBidderRepository, BidderRepository>();
builder.Services.AddScoped<IAuctionService, AuctionService>();

// Add TokenService to the DI container
builder.Services.AddSingleton(new TokenService(secretKey, issuer, audience));

// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddControllers();

// Configure Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Auction API", Version = "v1" });

    // Define the security scheme
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by a space and your JWT token."
    });

    // Apply the security scheme globally
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // Ensure authentication is enabled
app.UseAuthorization();

app.MapControllers();

app.Run();
public class TokenService
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;

    public TokenService(string secretKey, string issuer, string audience)
    {
        _secretKey = secretKey;
        _issuer = issuer;
        _audience = audience;
    }

    public string GenerateToken()
    {
        var keyBytes = Encoding.UTF8.GetBytes(_secretKey); // Use UTF8 bytes of the key
        var securityKey = new SymmetricSecurityKey(keyBytes);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "user"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            Expires = DateTime.Now.AddMinutes(30),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
