using AtowerDocElectronico.Aplicacion.Interfaces;
using AtowerDocElectronico.Aplicacion.Services.Autenticacion;
using AtowerDocElectronico.Aplicacion.Services.ConfiguracionAtowerNubex;
using AtowerDocElectronico.Aplicacion.Services.Factura;
using AtowerDocElectronico.Aplicacion.Services.Http;
using AtowerDocElectronico.Aplicacion.Services.Maestros_Parametros;
using AtowerDocElectronico.Aplicacion.Services.Rabbit;
using AtowerDocElectronico.Infraestructura.Exceptions;
using AtowerDocElectronico.Infraestructura.Interfaces;
using AtowerDocElectronico.Infraestructura.Persistencia.MySQL;
using AtowerDocElectronico.Infraestructura.Persistencia.Postgrest;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RabbitMQEventBus.BusRabbit;
using RabbitMQEventBus.EventoQueue;
using RabbitMQEventBus.Implement;
using System.Reflection;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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

builder.Services.AddDbContext<AtowerDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Atower"));
});

builder.Services.AddScoped<INubexDbContext, NubexDbContext>();
builder.Services.AddScoped<IAtowerDbContext, AtowerDbContext>();
builder.Services.AddScoped<IConsultasMaestrosNubexGet, ConsultasMaestrosNubexGet>();
builder.Services.AddScoped<ILogin, Login>();
builder.Services.AddScoped<IEnviarHttp, EnviarHttp>();
builder.Services.AddScoped<ICompañia, CompañiaServices>();
builder.Services.AddScoped<IEnviarFacturaRabbitNubex, EnviarFacturaRabbitNubex>();
builder.Services.AddScoped<IFacturaAtower, CrearFacturaAtower>();


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
builder.Services.AddTransient<CrearFacturaAtowerEventoManejador>();
builder.Services.AddTransient<IEventoManejador<CrearFacturaAtowerEventoQueue>, CrearFacturaAtowerEventoManejador>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(yourHandlerAssembly));


var app = builder.Build();
var serviceProvider = app.Services;

if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowSwaggerUI");
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Envios");
        c.OAuthClientId("swagger");
        c.OAuthClientSecret("secret");
        c.OAuthRealm("realm");
        c.OAuthAppName("Mi API");
    });
}

app.Use(async (context, next) =>
{
try
{
    // Primero llamas al siguiente middleware en la cadena
    await next();
}
catch (DbUpdateException ex)
{
    context.Response.StatusCode = 400;
    var result = JsonSerializer.Serialize(new
    {
        Mensaje = ex.Message
    });
    await context.Response.WriteAsync(result);
}
catch (UnprocessableEntityException ex)
{
    // Manejar excepciones aquí y devolver una respuesta JSON
    context.Response.StatusCode = 422;
    context.Response.ContentType = "application/json";

    var result = JsonSerializer.Serialize(new
    {
        Mensaje = $"Validacion de datos. ({ex.Message})"
    });

    await context.Response.WriteAsync(result);
}

catch (Exception ex)
{
    // Manejar excepciones aquí y devolver una respuesta JSON
    context.Response.StatusCode = 500;
    context.Response.ContentType = "application/json";

    var result = JsonSerializer.Serialize(new
    {
        Mensaje = $"Ah ocurrido un error inesperado en el servidor, por favor contactar al administrador del sistema. ({ex.Message})"
    });

    await context.Response.WriteAsync(result);
}
    // Luego verificas el código de estado de la respuesta
    if (context.Response.StatusCode == 401)
    {
        var result = JsonSerializer.Serialize(new { Mensaje = "No se ha autenticado para realizar este proceso." });
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(result);
    }
    else if (context.Response.StatusCode == 403)
    {
        var result = JsonSerializer.Serialize(new { Mensaje = "No tienes permiso para realizar esta acción." });
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(result);
    }
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
var eventBus = serviceProvider.GetRequiredService<IRabbitEventBus>();
eventBus.Subscribe<CrearFacturaAtowerEventoQueue, CrearFacturaAtowerEventoManejador>();
app.Run();
