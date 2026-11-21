
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProyectoZOGLEY.Custom;
using ProyectoZOGLEY.Data;
using ProyectoZOGLEY.Services;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// BASE DE DATOS
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// SERVICIOS
builder.Services.AddSingleton<Utilidades>();
builder.Services.AddScoped<UserInfoService>();
builder.Services.AddScoped<ComprasUsuarioService>();
builder.Services.AddScoped<FacturaService>();
builder.Services.AddScoped<CorreoFacturaService>();

// AUTENTICACIÓN JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme =
        JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],

        ClockSkew = TimeSpan.FromMinutes(1),

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                builder.Configuration["Jwt:Key"]!
            )
        ),

        RoleClaimType = ClaimTypes.Role,
        NameClaimType = ClaimTypes.NameIdentifier
    };
});

// AUTORIZACIÓN
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SoloAdmin",
        policy => policy.RequireRole("Admin"));

    options.AddPolicy("SoloCliente",
        policy => policy.RequireRole("Cliente"));
});

// CONTROLADORES Y SWAGGER
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// SWAGGER SOLO EN DESARROLLO
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CABECERAS PARA RESPUESTAS DE LA API
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/api"))
    {
        context.Response.Headers["Cache-Control"] =
            "no-store, no-cache, must-revalidate";

        context.Response.Headers["Pragma"] = "no-cache";
        context.Response.Headers["Expires"] = "0";
    }

    await next();
});

// HTTPS
app.UseHttpsRedirection();

// ARCHIVOS DEL FRONTEND
app.UseDefaultFiles();
app.UseStaticFiles();

// AUTENTICACIÓN Y AUTORIZACIÓN
app.UseAuthentication();
app.UseAuthorization();

// CONTROLADORES API
app.MapControllers();

app.Run();
