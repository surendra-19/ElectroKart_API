using ElectroKart.Common.Data;
using ElectroKart.DataAccess;
using ElectroKart.Service;
using Microsoft.EntityFrameworkCore;
using Scrutor;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Context>(options => 
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DatabaseConnection"),
        x => x.MigrationsAssembly("ElectroKart.Common")
        )
);

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
