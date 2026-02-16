using Api.Data;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using Api.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.RateLimiting;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);






// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insert: Bearer {token}"
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
//test


builder.Services.AddDbContext<AppDbContext>(options =>

{
    var cs = builder.Configuration.GetConnectionString("Default")
             ?? throw new InvalidOperationException("ConnectionString 'Default' not found.");
    options.UseSqlServer(cs);

});

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = ctx =>
    {
        ctx.ProblemDetails.Extensions["traceId"] = ctx.HttpContext.TraceIdentifier;
    };

});


var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "dotnet-api-starter";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "dotnet-api-starter";
var jwtKey = builder.Configuration["Jwt:Key"];

if(string.IsNullOrWhiteSpace(jwtKey))
{
    jwtKey = builder.Environment.IsDevelopment()
             ? "dev-only-key-change-me-dev-only-key-change-me"
             : throw new InvalidOperationException("Jwt:Key missing");

}

builder.Services.AddSingleton(new JwtSettings(jwtIssuer, jwtAudience, jwtKey));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
     .AddJwtBearer(o =>
     {
         o.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuer = true,
             ValidIssuer = jwtIssuer,
             ValidateAudience = true,
             ValidAudience = jwtAudience,
             ValidateIssuerSigningKey = true,
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
             ValidateLifetime = true,
             ClockSkew = TimeSpan.FromSeconds(30)
         };


     });

builder.Services.AddAuthorization();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>("database");



builder.Services.AddRateLimiter(o =>
{
    o.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 60;
        opt.Window = TimeSpan.FromMinutes(1);
    });


});



var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();



app.MapControllers();
app.MapHealthChecks("/health");
app.Run();


public partial class Program { }