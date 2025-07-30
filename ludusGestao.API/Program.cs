using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using System.Text;
using ludusGestao.Provider.Extensions;
using ludusGestao.Gerais.Extensions;
using ludusGestao.Autenticacao.Extensions;
using LudusGestao.Shared.Tenant;
using LudusGestao.Shared.Security;
using ludusGestao.API.Middleware;
using ludusGestao.Gestao.Extensions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Ludus Gestão API",
        Version = "v1",
        Description = "API para gestão de módulos Ludus"
    });

    // Configuração para autenticação JWT no Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
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

// Configuração do banco de dados
builder.Services.AddDbContext<ludusGestao.Provider.Data.Contexts.LudusGestaoReadDbContext>(options =>
    options.UseNpgsql(builder.Configuration["Database:Write:ConnectionString"], 
        npgsqlOptions => npgsqlOptions.EnableRetryOnFailure()));

builder.Services.AddDbContext<ludusGestao.Provider.Data.Contexts.LudusGestaoWriteDbContext>(options =>
    options.UseNpgsql(builder.Configuration["Database:Write:ConnectionString"], 
        npgsqlOptions => npgsqlOptions.EnableRetryOnFailure()));

// Adicionar módulos
builder.Services.AddProviderModule();
builder.Services.AddGeraisModule();
builder.Services.AddAutenticacaoModule();
builder.Services.AddGestaoServices();

// Registro de serviços compartilhados
builder.Services.AddScoped<LudusGestao.Shared.Notificacao.INotificador, LudusGestao.Shared.Notificacao.Notificador>();
builder.Services.AddScoped<ludusGestao.Autenticacao.Application.Services.JwtService>();
builder.Services.AddScoped<IPasswordHelper, BCryptPasswordHelper>();

// Configuração do Tenant
builder.Services.AddMemoryCache();
builder.Services.Configure<TenantResolverOptions>(builder.Configuration.GetSection("TenantResolver"));
builder.Services.AddScoped<ITenantContext, TenantContext>();
builder.Services.AddScoped<ITenantResolver, DefaultTenantResolver>();

// Adicionar registro do JwtSettings para DI
builder.Services.Configure<ludusGestao.Autenticacao.Application.Services.JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Configuração do JWT Bearer Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]))
        };
    });

builder.Services.Configure<Microsoft.AspNetCore.Mvc.ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var erros = context.ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        var resposta = new LudusGestao.Shared.Domain.Responses.RespostaBase(null, "Foram encontrados erros de validação.", erros)
        {
            Sucesso = false
        };

        return new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(resposta);
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Middleware de auditoria de tenant (deve vir antes da autenticação)
app.UseMiddleware<TenantAuditMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

// Middleware de tenant (deve vir após autenticação)
app.UseMiddleware<TenantMiddleware>();

app.MapControllers();

app.Run();
