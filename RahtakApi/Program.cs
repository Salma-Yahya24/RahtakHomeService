using Microsoft.EntityFrameworkCore;
using Interfaces;
using Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RahtakApi.DAL.Data;
using System.Text;
using Microsoft.OpenApi.Models;
using RahtakApi.Entities.Interfaces;
using RahtakApi.Services; // ✅ تأكد من إضافة الـ namespace الصحيح لـ EmailService

namespace RahtakApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ✅ إضافة خدمات الـ Controllers
            builder.Services.AddControllers();

            // ✅ إعداد قاعدة البيانات باستخدام SQL Server
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("con") ??
                throw new ArgumentNullException("Database connection string is missing")));

            // ✅ تسجيل UnitOfWork و Repository
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // ✅ تسجيل EmailService
            builder.Services.AddScoped<IEmailService, EmailService>();

            // ✅ إعداد JWT Authentication
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"] ?? throw new ArgumentNullException("JWT SecretKey is missing"));

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            builder.Services.AddAuthorization();

            // ✅ إضافة Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Rahtak API",
                    Version = "v1",
                    Description = "API documentation for Rahtak application"
                });

                // ✅ إضافة دعم لتوثيق API باستخدام JWT
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "ضع التوكن الخاص بك في هذا الحقل (Bearer {your token})"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new string[] { }
                    }
                });
            });

            // ✅ تفعيل CORS بدون Wildcard (*) وبدون تعارض مع AllowCredentials
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins",
                    policy => policy
                        .WithOrigins("http://localhost:4200", "https://yourdomain.com")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            var app = builder.Build();

            // ✅ دعم الملفات الثابتة (Static Files)
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            // ✅ تفعيل CORS باستخدام نفس اسم السياسة
            app.UseCors("AllowSpecificOrigins");

            // ✅ تفعيل Swagger فقط في وضع التطوير
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Rahtak API v1");
                    options.RoutePrefix = "swagger";
                });
            }

            // ✅ تفعيل Authentication & Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            // ✅ تفعيل التحكم في الـ API Controllers
            app.MapControllers();

            // ✅ تشغيل التطبيق
            app.Run();
        }
    }
}
