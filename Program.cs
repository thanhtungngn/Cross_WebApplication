using Cross_WebApplication.Configuration;
using Cross_WebApplication.Entity;
using Cross_WebApplication.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Cross_WebApplication.Repository.Abstract;
using Cross_WebApplication.Repository.Concrete;
using Cross_WebApplication.Context;
using Cross_WebApplication.Models;


var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenLocalhost(12001, listenOptions =>
    {
        listenOptions.UseHttps(httpsOptions =>
        {
            httpsOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
        });
    });
});

// Configure MongoDB 
var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>();
builder.Services.AddSingleton<IMongoDatabase>(serviceProvider =>
{
    var settings = serviceProvider.GetRequiredService<IOptions<MongoDbConfig>>().Value;
    var client = new MongoClient(settings.ConnectionString);
    return client.GetDatabase(settings.Name);
});

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
        .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>
        (
            mongoDbSettings.ConnectionString, mongoDbSettings.Name
        );

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = false;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.RequireUniqueEmail = true;
});

// Authentication Configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.Configure<JwtBearerOptions>(builder.Configuration.GetSection("Jwt"));

// Authorization

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AppConstant.Role.Admin, policy =>
        policy.RequireRole(AppConstant.Role.Admin));

    options.AddPolicy(AppConstant.Role.Contributor, policy =>
        policy.RequireRole(AppConstant.Role.Admin, AppConstant.Role.Contributor));

    options.AddPolicy(AppConstant.Role.Reader, policy =>
        policy.RequireRole(AppConstant.Role.Admin, AppConstant.Role.Contributor, AppConstant.Role.Reader));
});


// Dependency Injection
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<ITcpListenerService>(new TcpListenerService(int.Parse(builder.Configuration["TcpConfig:Port"] ?? "0")));

// CORS Config
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll",
    builder => builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader());
});

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
   
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
         "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
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

// Start TCP Listener
var tcpListener = app.Services.GetRequiredService<ITcpListenerService>();
tcpListener.Start();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAll"); // Use the CORS policy

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
