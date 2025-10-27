using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Products.Application.Abstractions.Interfaces;
using Products.Application.Commands;
using Products.Application.Mapping;
using Products.Application.Validators;
using Products.Infrastructure.Connection;
using Products.Infrastructure.Persistence;
using Products.SharedKernel;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateProductCommand>());
builder.Services.AddAutoMapper(cfg => { }, typeof(ProductProfile).Assembly);
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductValidator>();

// ======= JWT =======
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

// ======= Políticas =======
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));
});

// Dapper + DbInitializer
var connectionString = "Data Source=:memory:;Cache=Shared";
var connFactory = new SqliteConnectionFactory(connectionString);
DbInitializer.Initialize(connFactory.GetSharedConnection());
builder.Services.AddSingleton<IConnectionFactory>(connFactory);
builder.Services.AddScoped<IProductRepository, ProductRepositoryDapper>();

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
