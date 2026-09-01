using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using SB.Management.Application.Interfaces;
using SB.Management.Application.Services;
using SB.Management.Infrastructure.FileStorage;
using SB.Management.Infrastructure.Persistence;
using SB.Management.Infrastructure.Repositories;
using SB.Management.Infrastructure.Security;

namespace SB.Management.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // --- Serilog: logging a consola y archivo (requisito de especificaciones técnicas) ---
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseSerilog();

            // --- Base de datos (SQL Server para Empleado/Pago/Usuario/Rol) ---
            builder.Services.AddDbContext<SbGestionPagosDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // --- Inyección de dependencias: interfaz -> implementación concreta ---
            builder.Services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
            builder.Services.AddScoped<IPagoRepository, PagoRepository>();
            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            builder.Services.AddScoped<IEntidadGubernamentalRepository, EntidadGubernamentalFileRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddScoped<EmpleadoService>();
            builder.Services.AddScoped<EntidadGubernamentalService>();

            // --- JWT ---
            var claveJwt = builder.Configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("Falta 'Jwt:Key' en appsettings.json");

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
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(claveJwt))
                };
            });
            builder.Services.AddAuthorization();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opciones =>
            {
                opciones.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Ingresa: Bearer {tu token}"
                });
                opciones.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.AddCors(opciones =>
            {
                opciones.AddDefaultPolicy(politica =>
                    politica.WithOrigins("http://localhost:5173")
                            .AllowAnyHeader()
                            .AllowAnyMethod());
            });

            var app = builder.Build();

            // --- Manejo básico de excepciones (requisito de especificaciones técnicas) ---
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/error");
            }

            app.MapGet("/error", () => Results.Problem("Ocurrió un error inesperado en el servidor."));

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}