// Importación de namespaces necesarios para la configuración inicial
using Microsoft.AspNetCore.Authentication.Cookies;  // Autenticación basada en cookies
using TvShow.IOC;  // Configuración de inyección de dependencias personalizada
using TvShow.WebApi.Utilities.AutoMapper;  // Configuración de AutoMapper

// Creación del constructor de la aplicación web utilizando el nuevo estilo en .NET 6
var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor de servicios (IServiceCollection)

// Configuración de autenticación basada en cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/AccesoController/Login";  // Ruta para el inicio de sesión
        option.ExpireTimeSpan = TimeSpan.FromMinutes(20);  // Tiempo de expiración de la cookie
    });

// Configuración de inyección de dependencias utilizando métodos personalizados
builder.Services.InyectarDependencia(builder.Configuration);

// Configuración de AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));  // Registro del perfil de AutoMapper

// Agregar controladores MVC
builder.Services.AddControllers();

// Configuración de Swagger/OpenAPI para la documentación de API
builder.Services.AddEndpointsApiExplorer();  // Configuración de API Explorer
builder.Services.AddSwaggerGen();  // Configuración de Swagger

// Construcción de la aplicación
var app = builder.Build();

// Configuración del pipeline de solicitud HTTP

// Configuración para desarrollo: habilita Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();  // Habilita Swagger
    app.UseSwaggerUI();  // Habilita Swagger UI
}

app.UseHttpsRedirection();  // Redirección HTTPS

app.UseAuthorization();  // Configuración de autorización

app.MapControllers();  // Mapeo de los controladores MVC

app.Run();  // Ejecución de la aplicación