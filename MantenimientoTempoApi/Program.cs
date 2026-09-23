using MantenimientoTempoApi.Data;
using MantenimientoTempoApi.Repositories.Implementations;
using MantenimientoTempoApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace MantenimientoTempoApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Controladores
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // 2. Swagger con soporte para token JWT (botón Authorize)
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MantenimientoTempoApi", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Introduce el token JWT así: Bearer {tu_token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // Conexión a Turso
            builder.Services.AddSingleton<TursoDatabaseContext>();

            // Capa de Repositorios (Inyección de dependencias)
            builder.Services.AddScoped<IPlanesMantenimientoRepository, PlanesMantenimientoRepository>();
            builder.Services.AddScoped<IUsuariosRepository, UsuariosRepository>();
            builder.Services.AddScoped<IMaquinasRepository, MaquinasRepository>();
            builder.Services.AddScoped<IMantenimientosRepository, MantenimientosRepository>();
            builder.Services.AddScoped<IProduccionRepository, ProduccionRepository>();

            // 4. Configuración de Autenticación JWT
            var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key no configurada");
            var jwtIssuer = builder.Configuration["Jwt:Issuer"];
            var jwtAudience = builder.Configuration["Jwt:Audience"];

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
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // 5. Pipeline HTTP
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // El orden importa: primero Authentication y luego Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}