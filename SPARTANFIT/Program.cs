using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SPARTANFIT.Repository;
using SPARTANFIT.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Registro de dependencias
builder.Services.AddScoped<UsuarioRepository>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new UsuarioRepository(connectionString);
});

builder.Services.AddScoped<UsuarioService>();

builder.Services.AddScoped<PersonaRepository>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new PersonaRepository(connectionString);
});

// Registro del repositorio de recuperación de contraseña
builder.Services.AddScoped<Recuperacion_ContrasenaRepository>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new Recuperacion_ContrasenaRepository(connectionString);
});

builder.Services.AddScoped<EntrenadorRepository>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new EntrenadorRepository(connectionString);
});

builder.Services.AddScoped<EjercicioRepository>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new EjercicioRepository(connectionString);
});

builder.Services.AddScoped<AlimentoRepository>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new AlimentoRepository(connectionString);
});

builder.Services.AddScoped<RutinaRepository>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new RutinaRepository(connectionString);
});

builder.Services.AddScoped<PlanAlimenticioRepository>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new PlanAlimenticioRepository(connectionString);
});

builder.Services.AddScoped<PersonaService>();
builder.Services.AddScoped<AdministradorService>();
builder.Services.AddScoped<EntrenadorService>();
builder.Services.AddScoped<SPARTANFIT.Utilitys.CorreoUtility>();


// Configuración detallada de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SpartanFit API",
        Version = "v1",
        Description = "API para la gestión de usuarios y funcionalidades de SpartanFit.",
        Contact = new OpenApiContact
        {
            Name = "SpartanFit Support",
            Email = "spartanfitsoporte@gmail.com"
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SpartanFit API v1");
        c.RoutePrefix = string.Empty; // Hacer que Swagger esté en la raíz
    });
}

app.UseHttpsRedirection();

// Habilitar CORS
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
