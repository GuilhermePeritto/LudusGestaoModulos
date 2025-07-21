using ludusGestao.Provider.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddApplicationPart(typeof(ludusGestao.Eventos.API.Controllers.LocalController).Assembly);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adicionar serviços do LudusGestao
builder.Services.AddLudusGestaoProvider(builder.Configuration);

// Registrar automaticamente todos os serviços usando scan seguro
builder.Services.AddScopedFromAssembliesOf<
    ludusGestao.Eventos.Application.Service.Local.LocalService,
    ludusGestao.Eventos.Application.UseCases.Local.CriarLocalUseCase>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
