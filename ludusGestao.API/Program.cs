using ludusGestao.Provider.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddApplicationPart(typeof(ludusGestao.Eventos.API.Controllers.LocalController).Assembly);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "LudusGestao API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando o esquema Bearer. Exemplo: 'Bearer {token}'",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
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

// Adicionar serviços do LudusGestao
builder.Services.AddLudusGestaoProvider(builder.Configuration);

// Registrar automaticamente todos os serviços usando scan seguro
builder.Services.AddScopedFromAssembliesOf<
    ludusGestao.Eventos.Application.Service.Local.LocalService,
    ludusGestao.Eventos.Application.UseCases.Local.CriarLocalUseCase>();

builder.Services.AddScoped<LudusGestao.Shared.Application.Providers.ITenantContext, LudusGestao.Shared.Application.Providers.TenantContext>();
builder.Services.AddScoped<ludusGestao.Autenticacao.Application.Services.AutenticacaoService>();
builder.Services.AddScoped<ludusGestao.Autenticacao.Application.Services.JwtService>();
builder.Services.AddScoped<ludusGestao.Autenticacao.Application.UseCases.EntrarUseCase>();
builder.Services.AddScoped<ludusGestao.Autenticacao.Application.UseCases.AtualizarTokenUseCase>();
builder.Services.AddScoped<ludusGestao.Autenticacao.Application.Validations.EntrarValidation>();
builder.Services.AddScoped<ludusGestao.Autenticacao.Application.Validations.AtualizarTokenValidation>();

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

// Registrar UseCases e Services do módulo Gerais
builder.Services.AddScoped<ludusGestao.Gerais.Application.UseCases.Filial.CriarFilialUseCase>();
builder.Services.AddScoped<ludusGestao.Gerais.Application.UseCases.Filial.AtualizarFilialUseCase>();
builder.Services.AddScoped<ludusGestao.Gerais.Application.UseCases.Filial.RemoverFilialUseCase>();
builder.Services.AddScoped<ludusGestao.Gerais.Application.UseCases.Filial.BuscarFilialPorIdUseCase>();
builder.Services.AddScoped<ludusGestao.Gerais.Application.UseCases.Filial.ListarFiliaisUseCase>();
builder.Services.AddScoped<ludusGestao.Gerais.Application.UseCases.Empresa.CriarEmpresaUseCase>();
builder.Services.AddScoped<ludusGestao.Gerais.Application.UseCases.Empresa.AtualizarEmpresaUseCase>();
builder.Services.AddScoped<ludusGestao.Gerais.Application.UseCases.Empresa.RemoverEmpresaUseCase>();
builder.Services.AddScoped<ludusGestao.Gerais.Application.UseCases.Empresa.BuscarEmpresaPorIdUseCase>();
builder.Services.AddScoped<ludusGestao.Gerais.Application.UseCases.Empresa.ListarEmpresasUseCase>();
builder.Services.AddScoped<ludusGestao.Gerais.Application.UseCases.Usuario.CriarUsuarioUseCase>();
builder.Services.AddScoped<ludusGestao.Gerais.Application.UseCases.Usuario.AtualizarUsuarioUseCase>();
builder.Services.AddScoped<ludusGestao.Gerais.Application.UseCases.Usuario.RemoverUsuarioUseCase>();
builder.Services.AddScoped<ludusGestao.Gerais.Application.UseCases.Usuario.BuscarUsuarioPorIdUseCase>();
builder.Services.AddScoped<ludusGestao.Gerais.Application.UseCases.Usuario.ListarUsuariosUseCase>();
builder.Services.AddScoped<ludusGestao.Gerais.Application.Services.FilialService>();
builder.Services.AddScoped<ludusGestao.Gerais.Application.Services.EmpresaService>();
builder.Services.AddScoped<ludusGestao.Gerais.Application.Services.UsuarioService>();

// Providers Gerais
builder.Services.AddScoped<ludusGestao.Gerais.Domain.Providers.IFilialReadProvider, ludusGestao.Provider.Data.Providers.Gerais.Filial.FilialPostgresReadProvider>();
builder.Services.AddScoped<ludusGestao.Gerais.Domain.Providers.IFilialWriteProvider, ludusGestao.Provider.Data.Providers.Gerais.Filial.FilialPostgresWriteProvider>();
builder.Services.AddScoped<ludusGestao.Gerais.Domain.Providers.IEmpresaReadProvider, ludusGestao.Provider.Data.Providers.Gerais.Empresa.EmpresaPostgresReadProvider>();
builder.Services.AddScoped<ludusGestao.Gerais.Domain.Providers.IEmpresaWriteProvider, ludusGestao.Provider.Data.Providers.Gerais.Empresa.EmpresaPostgresWriteProvider>();
builder.Services.AddScoped<ludusGestao.Gerais.Domain.Providers.IUsuarioReadProvider, ludusGestao.Provider.Data.Providers.Gerais.Usuario.UsuarioPostgresReadProvider>();
builder.Services.AddScoped<ludusGestao.Gerais.Domain.Providers.IUsuarioWriteProvider, ludusGestao.Provider.Data.Providers.Gerais.Usuario.UsuarioPostgresWriteProvider>();

// Services e UseCases do módulo Eventos
builder.Services.AddScoped<ludusGestao.Eventos.Application.Service.Local.LocalService>();
builder.Services.AddScoped<ludusGestao.Eventos.Application.UseCases.Local.CriarLocalUseCase>();
builder.Services.AddScoped<ludusGestao.Eventos.Application.UseCases.Local.AtualizarLocalUseCase>();
builder.Services.AddScoped<ludusGestao.Eventos.Application.UseCases.Local.RemoverLocalUseCase>();
builder.Services.AddScoped<ludusGestao.Eventos.Application.UseCases.Local.BuscarLocalPorIdUseCase>();
builder.Services.AddScoped<ludusGestao.Eventos.Application.UseCases.Local.ListarLocaisUseCase>();
builder.Services.AddScoped<ludusGestao.Eventos.Domain.Providers.ILocalReadProvider, ludusGestao.Provider.Data.Providers.Eventos.Local.LocalPostgresReadProvider>();
builder.Services.AddScoped<ludusGestao.Eventos.Domain.Providers.ILocalWriteProvider, ludusGestao.Provider.Data.Providers.Eventos.Local.LocalPostgresWriteProvider>();

builder.Services.AddSingleton<LudusGestao.Shared.Application.Events.IEventBus, LudusGestao.Shared.Application.Events.EventBus>();
builder.Services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();

// Registrar handlers globais de erro
var eventBus = builder.Services.BuildServiceProvider().GetRequiredService<LudusGestao.Shared.Application.Events.IEventBus>();
var accessor = builder.Services.BuildServiceProvider().GetRequiredService<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
ludusGestao.API.Extensions.EventBusExtensions.RegistrarHandlersGlobais(eventBus, accessor);

builder.Services.Configure<Microsoft.AspNetCore.Mvc.ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var erros = context.ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        var resposta = new LudusGestao.Shared.Application.Responses.RespostaBase<object>(null)
        {
            Sucesso = false,
            Mensagem = "Foram encontrados erros de validação.",
            Erros = erros
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
app.UseAuthentication();
app.UseMiddleware<ludusGestao.API.Middleware.TenantMiddleware>();
app.UseMiddleware<ludusGestao.API.Middleware.ExceptionMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
