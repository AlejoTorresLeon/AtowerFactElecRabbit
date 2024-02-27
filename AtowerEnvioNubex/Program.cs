using AtowerEnvioNubex.Aplicacion.Interfaces;
using AtowerEnvioNubex.Aplicacion.Services;
using AtowerEnvioNubex.Infraestructura.Interfaces;
using AtowerEnvioNubex.Infraestructura.Persistencia.MySQL;
using AtowerEnvioNubex.Presentacion.Rabbit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RabbitMQEventBus.BusRabbit;
using RabbitMQEventBus.EventoQueue;
using RabbitMQEventBus.Implement;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSwaggerUI",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Envios Electronicos", Version = "v1" });

    // Configuración para la autenticación con JWT en Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
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
            new List<string>()
        }
    });
});

builder.Services.AddDbContext<NubexDbContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("Nubex"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Nubex")));
});

builder.Services.AddScoped<INubexDbContext, NubexDbContext>();
builder.Services.AddScoped<IEnviarHttp, EnviarHttp>();
builder.Services.AddScoped<IEnvioFacturaNubexDian, EnvioFacturaNubexDian>();

var jwtKey = builder.Configuration["JWT:key"];
var encryptionKey = builder.Configuration["JWT:encryptionKey"];
if (jwtKey != null && encryptionKey != null)
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(encryptionKey)) // Agrega la clave de cifrado
            };
        });
}
else
{
    throw new InvalidOperationException("La configuración del token de seguridad JWT:key o JWT:encryptionKey no está especificada en el archivo de configuración.");
}

Assembly yourHandlerAssembly = typeof(Program).Assembly;
builder.Services.AddSingleton<IRabbitEventBus, RabbitEventBus>(sp =>
{
    var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
    return new RabbitEventBus(sp.GetService<IMediator>(), scopeFactory);
});
builder.Services.AddTransient<FacturaEventoManejador>();
builder.Services.AddTransient<IEventoManejador<FacturaEventoQueue>, FacturaEventoManejador>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(yourHandlerAssembly));

var app = builder.Build();
var serviceProvider = app.Services;

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
var eventBus = serviceProvider.GetRequiredService<IRabbitEventBus>();
eventBus.Subscribe<FacturaEventoQueue, FacturaEventoManejador>();
app.Run();
