using ludusGestao.Provider.Extensions;

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
            Mensagem = string.Join("; ", erros)
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
app.UseMiddleware<ludusGestao.API.Middleware.TenantMiddleware>();
app.UseMiddleware<ludusGestao.API.Middleware.ExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
