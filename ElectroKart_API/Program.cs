using ElectroKart.Common.Configuration;
using ElectroKart.Common.Data;
using ElectroKart.Common.JwtConfiguration;
using ElectroKart.DataAccess;
using ElectroKart.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scrutor;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<JwtSettings>(
        builder.Configuration.GetSection("JwtSettings")
    );
// Add services to the container.
builder.Services.Scan(scan => scan
    .FromAssemblies(
        typeof(AuthorizationService).Assembly,     
        typeof(AuthorizationDataAccess).Assembly   
    )
    .AddClasses(classes => classes.Where(t =>
        t.Name.EndsWith("Service") || t.Name.EndsWith("DataAccess")))
    .AsSelf()
    .WithScopedLifetime()
);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
    };
});
builder.Services.AddAuthorization();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( res =>
{
    res.SwaggerDoc("v1", new OpenApiInfo { Title = "ElectroKart API", Version = "v1" });

    res.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    res.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});
builder.Services.AddDbContext<Context>(options => 
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DatabaseConnection"),
        x => x.MigrationsAssembly("ElectroKart.Common")
        )
);
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
