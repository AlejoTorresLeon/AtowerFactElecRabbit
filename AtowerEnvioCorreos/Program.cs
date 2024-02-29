using AtowerEnvioCorreos.Aplicacion.Interfaces;
using AtowerEnvioCorreos.Aplicacion.Services;
using AtowerEnvioCorreos.ManejadorRabbit;
using MediatR;
using Microsoft.OpenApi.Models;
using RabbitMQEventBus.BusRabbit;
using RabbitMQEventBus.EventoQueue;
using RabbitMQEventBus.Implement;
using System.Reflection;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddControllers();
//builder.Services.AddSignalR();



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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
                .WithOrigins()  // Reemplaza con el dominio de tu aplicación frontend
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials() // Habilita las credenciales
                .SetIsOriginAllowed(_ => true);  // Permite cualquier origen
        });
});


//builder.Services.Configure<ApiBehaviorOptions>(options =>
//{
//    options.InvalidModelStateResponseFactory = actionContext =>
//    {
//        var errorsInModelState = actionContext.ModelState
//            .Where(x => x.Value.Errors.Count > 0)
//            .ToDictionary(
//                kvp => kvp.Key,
//                kvp => kvp.Value.Errors
//                    .Select(e => e.ErrorMessage.Contains("required") ? "requerido" :
//                                 e.ErrorMessage)
//                    .FirstOrDefault()
//            );

//        return new ObjectResult(errorsInModelState)
//        {
//            StatusCode = StatusCodes.Status422UnprocessableEntity
//        };
//    };
//});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{

    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AudioGuia", Version = "v1" });
    //c.ExampleFilters();
    c.EnableAnnotations();

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



Assembly yourHandlerAssembly = typeof(Program).Assembly;
builder.Services.AddSingleton<IRabbitEventBus, RabbitEventBus>(sp =>
{
    var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
    return new RabbitEventBus(sp.GetService<IMediator>(), scopeFactory);
});
builder.Services.AddTransient<EmailEventoManejador>();

builder.Services.AddTransient<IEventoManejador<EmailEventoQueue>, EmailEventoManejador>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(yourHandlerAssembly));


builder.Services.AddScoped<ICorreoService, SmtpCorreoService>();





var app = builder.Build();

var serviceProvider = app.Services;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (context, next) =>
{
    try
    {
        // Primero llamas al siguiente middleware en la cadena
        await next();
    }
    //catch (DbUpdateException ex)
    //{
    //    context.Response.StatusCode = 400;
    //    var result = JsonSerializer.Serialize(new
    //    {
    //        Mensaje = ex.Message
    //    });
    //    await context.Response.WriteAsync(result);
    //}

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
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

var eventBus = serviceProvider.GetRequiredService<IRabbitEventBus>();
eventBus.Subscribe<EmailEventoQueue, EmailEventoManejador>();
app.Run();
