// Importaci�n de namespaces necesarios para la configuraci�n inicial
using Microsoft.AspNetCore.Authentication.Cookies;  // Autenticaci�n basada en cookies
using TvShow.IOC;  // Configuraci�n de inyecci�n de dependencias personalizada
using TvShow.WebApi.Utilities.AutoMapper;  // Configuraci�n de AutoMapper

// Creaci�n del constructor de la aplicaci�n web utilizando el nuevo estilo en .NET 6
var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor de servicios (IServiceCollection)

// Configuraci�n de autenticaci�n basada en cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/AccesoController/Login";  // Ruta para el inicio de sesi�n
        option.ExpireTimeSpan = TimeSpan.FromMinutes(20);  // Tiempo de expiraci�n de la cookie
    });

// Configuraci�n de inyecci�n de dependencias utilizando m�todos personalizados
builder.Services.InyectarDependencia(builder.Configuration);

// Configuraci�n de AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));  // Registro del perfil de AutoMapper

// Agregar controladores MVC
builder.Services.AddControllers();

// Configuraci�n de Swagger/OpenAPI para la documentaci�n de API
builder.Services.AddEndpointsApiExplorer();  // Configuraci�n de API Explorer
builder.Services.AddSwaggerGen();  // Configuraci�n de Swagger

// Construcci�n de la aplicaci�n
var app = builder.Build();

// Configuraci�n del pipeline de solicitud HTTP

// Configuraci�n para desarrollo: habilita Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();  // Habilita Swagger
    app.UseSwaggerUI();  // Habilita Swagger UI
}

app.UseHttpsRedirection();  // Redirecci�n HTTPS

app.UseAuthorization();  // Configuraci�n de autorizaci�n

app.MapControllers();  // Mapeo de los controladores MVC

app.Run();  // Ejecuci�n de la aplicaci�n