using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;
using System.Text;
using WebTiemVangKimCuc.SER.Domain.SeedWork;
using WebTiemVangKimCuc.SER.Domain.Services;
using WebTiemVangKimCuc.SER.Extensions;
using WebTiemVangKimCuc.SER.Infrastructure;
using WebTiemVangKimCuc.SER.Middlewares;
using WebTiemVangKimCuc.SER.ViewModel;
using static WebTiemVangKimCuc.SER.Domain.SeedWork.ISeedService;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

//Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          //policy.WithOrigins("https://tiemvangkimcuc.vercel.app", "http://183.80.65.112", "https://183.80.65.112")
                          policy.AllowAnyOrigin()      
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

//Add Logger
//builder.Services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

//Add AutoMapper
var config = new MapperConfiguration(cfg =>
{
    cfg.AllowNullCollections = true;
    cfg.AddProfile(new MapperInitializer());
});
var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);


// Dependency Injection
builder.Services.AddScoped<ISeedService, SeedService>();

// Connect SQL
var connectionString = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");

builder.Services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(connectionString
    ));


// Add Authentication
builder.Services.AddHttpContextAccessor();
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
        ValidIssuer = Environment.GetEnvironmentVariable("ISSUER"),
        ValidAudience = Environment.GetEnvironmentVariable("AUDIENCE"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("KEY")))
    };
});
builder.Services.AddTransient<GlobalExeptionHandlers>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// Add Lockers to Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Kim Cuc APIs", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Use bearer token to authorize",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.OperationFilter<AddAuthHeaderOperationFilter>();
});

var app = builder.Build();

// Return code 401 with custom message
//https://learn.microsoft.com/en-us/answers/questions/1281126/how-to-catch-webapi-core-unauthenticated-status
app.UseStatusCodePages(async statusCodeContext =>
{
    switch (statusCodeContext.HttpContext.Response.StatusCode)
    {
        case 401:
            statusCodeContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await statusCodeContext.HttpContext.Response.WriteAsJsonAsync(new ResultApi("Authentication failed. Please provide valid token."));

            break;
        case 403:
            statusCodeContext.HttpContext.Response.StatusCode = 400;
            await statusCodeContext.HttpContext.Response.WriteAsJsonAsync(new ResultApi("Authorization failed. User does not allow to access resource."));
            break;
    }
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
    c.RoutePrefix = string.Empty; // Set Swagger UI as the root
});

app.UseStaticFiles();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

// Use MiddleWare to Handle Exception
app.UseMiddleware<GlobalExeptionHandlers>();

app.MapControllers();

app.Run();
