using System.Text;
using CalculatorApi.Authentication;
using CalculatorApi.Middleware;
using CalculatorApi.Services;
using CalculatorApi.Services.Interfaces;
using CalculatorApi.Services.Operations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------------
// Load JWT settings from user secrets in development or environment variables in Docker
// ----------------------------------------------------------

// Add User Secrets only in Development Environment
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}
else
{
    // In Docker or other environments, use environment variables
    builder.Configuration.AddEnvironmentVariables();
}

// Load JwtSettings from Configuration
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

if (jwtSettings == null)
{
    throw new InvalidOperationException("JwtSettings configuration is missing.");
}

// ----------------------------------------------------------
// JWT Token Generator - Singleton
// ----------------------------------------------------------
builder.Services.AddSingleton<JwtTokenGenerator>();

// ----------------------------------------------------------
// Register Calculator Operations
// ----------------------------------------------------------
builder.Services.AddTransient<IOperation, AddOperation>();
builder.Services.AddTransient<IOperation, SubtractOperation>();
builder.Services.AddTransient<IOperation, MultiplyOperation>();
builder.Services.AddTransient<IOperation, DivideOperation>();
builder.Services.AddTransient<IOperationFactory, OperationFactory>();

builder.Services.AddScoped<ICalculatorService, CalculatorService>();

// ----------------------------------------------------------
// Controllers - Global Authorization Policy (require auth)
// ----------------------------------------------------------
builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
})
.AddNewtonsoftJson(options =>
{
    // Enums will be serialized as strings (e.g., "Add" instead of 0)
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
});

// ----------------------------------------------------------
// JWT Authentication Configuration
// ----------------------------------------------------------
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
    };

    // Add custom logic when authentication fails
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception is SecurityTokenExpiredException)
            {
                context.Response.Headers.Append("Token-Expired", "true");
            }
            return Task.CompletedTask;
        }
    };
});

// ----------------------------------------------------------
// Swagger/OpenAPI Configuration
// ----------------------------------------------------------
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Calculator API", Version = "v1" });

    // Add JWT Authentication to Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter JWT token with 'Bearer' prefix (e.g. Bearer {token})",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

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

// ----------------------------------------------------------
// Build Application
// ----------------------------------------------------------
var app = builder.Build();

// ----------------------------------------------------------
// Development Tools
// ----------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// ----------------------------------------------------------
// Global Error Handling Middleware
// ----------------------------------------------------------
app.UseMiddleware<ExceptionHandlingMiddleware>();

// ----------------------------------------------------------
// Swagger UI
// ----------------------------------------------------------
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Calculator API v1");
});

// ----------------------------------------------------------
// Middleware Pipeline
// ----------------------------------------------------------
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// ----------------------------------------------------------
// Endpoint Mapping
// ----------------------------------------------------------
app.MapControllers();

// ----------------------------------------------------------
// Start the Web Application
// ----------------------------------------------------------
app.Run();
