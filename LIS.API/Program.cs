using APiUsers.Data;
using APiUsers.Repository;
using APiUsers.Repository.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ===== ≈÷«›… Controllers + ≈⁄œ«œ«  JSON =====
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // «· ⁄«„· „⁄ «·Õ·ﬁ«  «·„—Ã⁄Ì…
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.MaxDepth = 64;

        // œ⁄„ «·√Õ—› «·⁄—»Ì…
        options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
    });

// ===== ≈⁄œ«œ«  CORS =====
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFlutterWeb", policy =>
    {
        policy.WithOrigins("http://localhost:5000") // €Ì¯— «·»Ê—  Õ”»  ÿ»Ìﬁ Flutter Web
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ===== ≈⁄œ«œ«  ﬁ«⁄œ… «·»Ì«‰«  =====
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Data Source=DESKTOP-T4D38G7\\SQLEXPRESS;Initial Catalog=LaboratoryInformationSystem;Integrated Security=True;Trust Server Certificate=True"));

// ===== ≈÷«›… Repository =====
builder.Services.AddTransient(typeof(IReopsitory<>), typeof(MainRepository<>));

// ===== Swagger =====
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // œ⁄„ JWT ›Ì Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "√œŒ· JWT token Â‰«"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// ===== ≈⁄œ«œ«  JWT Authentication =====
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "LIS",
            ValidAudience = "sa@gmail.com",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsAReallyLongSecretKeyForJwtToken12345"))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// ===== Middleware =====
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFlutterWeb"); // ﬁ»· Authentication

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
