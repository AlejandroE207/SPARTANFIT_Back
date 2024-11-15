using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SPARTANFIT.Repository;
using SPARTANFIT.Services;
using SPARTANFIT.Utilitys;
using System.Text;


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
        Description = @"
<h2>SpartanFit API</h2>
<p>Bienvenido a la API de SpartanFit, diseñada para ayudar a los usuarios a alcanzar sus objetivos de fitness y nutrición mediante herramientas digitales avanzadas.</p>

<h3>¿Qué es SpartanFit?</h3>
<p>SpartanFit es una plataforma integral que permite a los usuarios:</p>
<ul>
    <li>Realizar un seguimiento personalizado de sus entrenamientos en el gimnasio.</li>
    <li>Planificar rutinas de ejercicio adaptadas a sus objetivos.</li>
    <li>Recibir recomendaciones nutricionales basadas en su progreso y necesidades específicas.</li>
</ul>

<h3>Funcionalidades de la API</h3>
<ul>
    <li><strong>Gestión de usuarios:</strong> Registro, inicio de sesión y administración de perfiles.</li>
    <li><strong>Seguimiento de entrenamientos:</strong> Información sobre ejercicios, tiempos, repeticiones y métricas de progreso.</li>
    <li><strong>Planificación nutricional:</strong> Creación de planes alimenticios y registro de ingesta diaria.</li>
    <li><strong>Alertas personalizadas:</strong> Notificaciones para mantener la motivación y los objetivos.</li>
    <li><strong>Reportes:</strong> Descarga y envío por correo de reportes de usuario y entrenadores registrados en la plataforma.</li>
</ul>

<h3>Manuales</h3>
<ul>
    <li><a href='https://drive.google.com/file/d/1vAor6oHKtDlf-HRkB1LNKK25kdUxoEX8/view?usp=sharing' target='_blank'>Manual de usuario</a></li>
    <li><a href='https://drive.google.com/file/d/1QTIfYfDe3XQa-sv6K1C-omvnnpnYQ2g1/view?usp=sharing' target='_blank'>Manual técnico</a></li>
    <li><a href='https://youtu.be/igp0Zs3-lTo' target='_blank'>Video comercial</a></li>
</ul>
",

    Contact = new OpenApiContact
        {
            Name = "SpartanFit Support",
            Email = "spartanfitsoporte@gmail.com",
            Url = new Uri("https://spartanfit.com")
        },
        License = new OpenApiLicense
        {
            Name = "Licencia MIT",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });
});


builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Ingrese el token JWT en el formato: Bearer {token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var SecretKey = builder.Configuration["Key:secretKey"];
if (string.IsNullOrEmpty(SecretKey))
{
    throw new InvalidOperationException("SecretKey no está configurada correctamente.");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)),
        ValidateIssuer = false,
        ValidateAudience = false,
        RequireExpirationTime = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-Expired-Time", "true");
            }
            return Task.CompletedTask;
        }
    };
});

// Registra TokenUtility con la clave desde configuración
builder.Services.AddScoped<TokenUtility>(provider =>
    new TokenUtility(SecretKey));


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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
