# Arquitetura do Módulo Eventos - Padrão para Módulos

## Visão Geral

Este documento descreve a arquitetura do módulo de eventos que serve como padrão para todos os outros módulos do sistema LudusGestao. A arquitetura segue os princípios de Clean Architecture, Domain-Driven Design (DDD) e SOLID.

## Estrutura de Pastas

```
ludusGestao.Eventos/
├── API/
│   └── Controllers/
│       └── LocalController.cs
├── Application/
│   └── Services/
│       └── LocalService.cs
├── Domain/
│   └── Local/
│       ├── DTOs/
│       │   ├── LocalDTO.cs
│       │   ├── CriarLocalDTO.cs
│       │   └── AtualizarLocalDTO.cs
│       ├── Interfaces/
│       │   ├── ILocalService.cs
│       │   ├── ICriarLocalUseCase.cs
│       │   ├── IAtualizarLocalUseCase.cs
│       │   ├── IRemoverLocalUseCase.cs
│       │   ├── IBuscarLocalPorIdUseCase.cs
│       │   ├── IListarLocaisUseCase.cs
│       │   ├── ILocalReadProvider.cs
│       │   └── ILocalWriteProvider.cs
│       ├── Local.cs
│       ├── Specifications/
│       │   └── LocalAtivoSpecification.cs
│       ├── UseCases/
│       │   ├── CriarLocalUseCase.cs
│       │   ├── AtualizarLocalUseCase.cs
│       │   ├── RemoverLocalUseCase.cs
│       │   ├── BuscarLocalPorIdUseCase.cs
│       │   └── ListarLocaisUseCase.cs
│       └── Validations/
│           ├── CriarLocalValidation.cs
│           └── AtualizarLocalValidation.cs
└── ludusGestao.Eventos.csproj
```

## Camadas da Arquitetura

### 1. API Layer (Camada de Apresentação)

**Responsabilidade**: Exposição de endpoints REST, validação de entrada e formatação de resposta.

#### Controller
- Herda de `ControllerRestBase` (classe base compartilhada)
- Implementa endpoints REST padrão (GET, POST, PUT, DELETE)
- Utiliza injeção de dependência para serviços
- Retorna respostas padronizadas usando `CustomResponse`

```csharp
[ApiController]
[Route("api/locais")]
public class LocalController : ControllerRestBase
{
    private readonly ILocalService _service;

    public LocalController(INotificador notificador, ILocalService service)
        : base(notificador)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] QueryParamsBase query)
    {
        var result = await _service.Listar(query);
        return CustomResponse(HttpStatusCode.OK, result, "Locais listados com sucesso.");
    }
}
```

### 2. Application Layer (Camada de Aplicação)

**Responsabilidade**: Orquestração de casos de uso, coordenação entre domínio e infraestrutura.

#### Service
- Implementa a interface do domínio
- Orquestra os casos de uso
- Converte DTOs de entrada em entidades do domínio
- Converte entidades em DTOs de saída

```csharp
public class LocalService : BaseService, ILocalService
{
    private readonly ICriarLocalUseCase _criarUseCase;
    // ... outros use cases

    public async Task<LocalDTO> Criar(CriarLocalDTO dto)
    {
        var local = Local.Criar(dto.Nome, dto.Rua, dto.Numero, dto.Bairro, dto.Cidade, dto.Estado, dto.Cep, dto.Capacidade);
        var localCriado = await _criarUseCase.Executar(local);
        return LocalDTO.Criar(localCriado);
    }
}
```

### 3. Domain Layer (Camada de Domínio)

**Responsabilidade**: Regras de negócio, entidades, casos de uso e validações.

#### Entidade
- Herda de `EntidadeBase`
- Implementa métodos de criação, atualização e remoção
- Encapsula regras de negócio
- Utiliza Value Objects para propriedades complexas

```csharp
public class Local : EntidadeBase
{
    public string Nome { get; private set; }
    public Endereco Endereco { get; private set; }
    public int Capacidade { get; private set; }
    public bool Ativo { get; private set; }
    public int TenantId { get; private set; }

    public static Local Criar(string nome, string rua, string numero, string bairro, string cidade, string estado, string cep, int capacidade)
    {
        return new Local
        {
            Nome = nome,
            Endereco = new Endereco(rua, numero, bairro, cidade, estado, cep),
            Capacidade = capacidade,
            Ativo = true,
        };
    }

    public void Atualizar(string nome, string rua, string numero, string bairro, string cidade, string estado, string cep, int capacidade)
    {
        Nome = nome;
        Endereco = new Endereco(rua, numero, bairro, cidade, estado, cep);
        Capacidade = capacidade;
    }

    public void Remover()
    {
        Ativo = false;
    }
}
```

#### DTOs
- **LocalDTO**: DTO de resposta com todos os dados da entidade
- **CriarLocalDTO**: DTO de entrada para criação com validações
- **AtualizarLocalDTO**: DTO de entrada para atualização

```csharp
public class LocalDTO
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    // ... outras propriedades

    public static LocalDTO Criar(Local local)
    {
        return new LocalDTO
        {
            Id = local.Id,
            Nome = local.Nome,
            // ... mapeamento das propriedades
        };
    }
}
```

#### Use Cases
- Herdam de `BaseUseCase`
- Implementam regras de negócio específicas
- Utilizam providers para persistência
- Executam validações antes de processar

```csharp
public class CriarLocalUseCase : BaseUseCase
{
    private readonly ILocalWriteProvider _provider;
    
    public CriarLocalUseCase(ILocalWriteProvider provider, INotificador notificador)
        : base(notificador)
    {
        _provider = provider;
    }

    public async Task<Local> Executar(Local local)
    {
        if (!ExecutarValidacao(new CriarLocalValidation(), local))
            return null;

        await _provider.Adicionar(local);
        await _provider.SalvarAlteracoes();
        return local;
    }
}
```

#### Interfaces
- Definem contratos para serviços e casos de uso
- Seguem o princípio de inversão de dependência
- Permitem testabilidade e flexibilidade

```csharp
public interface ILocalService
{
    Task<LocalDTO> Criar(CriarLocalDTO dto);
    Task<LocalDTO> Atualizar(Guid id, AtualizarLocalDTO dto);
    Task<bool> Remover(Guid id);
    Task<LocalDTO> BuscarPorId(Guid id);
    Task<IEnumerable<LocalDTO>> Listar(QueryParamsBase query);
}
```

#### Specifications
- Encapsulam regras de negócio reutilizáveis
- Implementam padrão Specification
- Facilitam testes e manutenção

```csharp
public class LocalAtivoSpecification
{
    public string MensagemErro => "O local precisa estar ativo para ser atualizado.";
    public bool IsSatisfiedBy(Local local)
    {
        return local.Ativo;
    }
}
```

#### Validations
- Utilizam FluentValidation
- Validam entidades antes de persistência
- Integram com specifications

```csharp
public class CriarLocalValidation : AbstractValidator<Local>
{
    public CriarLocalValidation()
    {
        RuleFor(l => l.Nome)
            .NotEmpty().WithMessage("O nome do local é obrigatório.");
        RuleFor(l => l.Capacidade)
            .GreaterThan(0).WithMessage("A capacidade deve ser maior que zero.");
        RuleFor(l => l)
            .Must(l => new LocalAtivoSpecification().IsSatisfiedBy(l))
            .WithMessage("O local precisa estar ativo para ser criado.");
    }
}
```

## Infraestrutura (Provider Layer)

### Providers
- **ReadProvider**: Herda de `ProviderBase` para operações de leitura
- **WriteProvider**: Implementa operações de escrita específicas

```csharp
public class LocalPostgresReadProvider : ProviderBase<Local>, ILocalReadProvider
{
    public LocalPostgresReadProvider(LudusGestaoReadDbContext context) : base(context)
    {
    }
}

public class LocalPostgresWriteProvider : ILocalWriteProvider
{
    private readonly LudusGestaoWriteDbContext _context;
    
    public async Task Adicionar(LocalEntity local)
    {
        await _context.Locais.AddAsync(local);
    }

    public async Task<int> SalvarAlteracoes()
    {
        return await _context.SaveChangesAsync();
    }
}
```

### Configurações
- Configuram mapeamento Entity Framework
- Definem constraints e índices
- Configuram Value Objects

```csharp
public class LocalConfiguration : IEntityTypeConfiguration<Local>
{
    public void Configure(EntityTypeBuilder<Local> builder)
    {
        builder.ToTable("Locais");
        builder.HasKey(l => l.Id);
        
        builder.Property(l => l.Nome)
            .IsRequired()
            .HasMaxLength(200);

        // Configuração do Value Object Endereco
        builder.OwnsOne(l => l.Endereco, endereco =>
        {
            endereco.Property(e => e.Rua)
                .HasColumnName("Rua")
                .HasMaxLength(200)
                .IsRequired();
            // ... outras propriedades
        });
    }
}
```

## Dependências

### Package References
```xml
<ItemGroup>
    <PackageReference Include="FluentValidation" Version="11.8.1" />
    <PackageReference Include="FluentValidation.DependencyInjectionExtensions" Version="11.8.1" />
    <PackageReference Include="Microsoft.AspNetCore.Mvc.Core" Version="2.3.0" />
</ItemGroup>

<ItemGroup>
    <ProjectReference Include="..\ludusGestao.Shared\ludusGestao.Shared.csproj" />
</ItemGroup>
```

## Padrões de Nomenclatura

### Arquivos e Pastas
- **Entidades**: Nome singular (ex: `Local.cs`)
- **DTOs**: Sufixo `DTO` (ex: `LocalDTO.cs`, `CriarLocalDTO.cs`)
- **Use Cases**: Sufixo `UseCase` (ex: `CriarLocalUseCase.cs`)
- **Interfaces**: Prefixo `I` (ex: `ILocalService.cs`)
- **Validations**: Sufixo `Validation` (ex: `CriarLocalValidation.cs`)
- **Specifications**: Sufixo `Specification` (ex: `LocalAtivoSpecification.cs`)
- **Providers**: Sufixo `Provider` (ex: `LocalPostgresReadProvider.cs`)

### Namespaces
- Seguem a estrutura de pastas
- Exemplo: `ludusGestao.Eventos.Domain.Local.UseCases`

## Fluxo de Dados

1. **Request** → Controller
2. **Controller** → Service (Application Layer)
3. **Service** → Use Case (Domain Layer)
4. **Use Case** → Provider (Infrastructure Layer)
5. **Provider** → Database
6. **Response** → Controller → Client

## Regras de Negócio

### Validações
- Validações de entrada nos DTOs (Data Annotations)
- Validações de domínio nas entidades
- Validações de negócio nos Use Cases (FluentValidation)
- Specifications para regras reutilizáveis

### Auditoria
- Todas as entidades herdam de `EntidadeBase`
- Campos automáticos: `Id`, `DataCriacao`, `DataAlteracao`
- Suporte a multi-tenancy com `TenantId`

### Soft Delete
- Entidades não são fisicamente removidas
- Campo `Ativo` controla visibilidade
- Método `Remover()` apenas desativa a entidade

## Testes

### Estrutura de Testes
```
ludusGestao.Tests/
├── Eventos/
│   ├── Domain/
│   │   ├── Local/
│   │   │   ├── UseCases/
│   │   │   ├── Validations/
│   │   │   └── Specifications/
│   │   └── Services/
│   └── API/
│       └── Controllers/
```

### Tipos de Testes
- **Unit Tests**: Use Cases, Validations, Specifications
- **Integration Tests**: Services, Controllers
- **End-to-End Tests**: Fluxos completos

## Configuração de Dependências

### Service Collection Extensions
```csharp
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEventosModule(this IServiceCollection services)
    {
        // Registro de Use Cases
        services.AddScoped<ICriarLocalUseCase, CriarLocalUseCase>();
        services.AddScoped<IAtualizarLocalUseCase, AtualizarLocalUseCase>();
        // ... outros use cases

        // Registro de Services
        services.AddScoped<ILocalService, LocalService>();

        // Registro de Providers
        services.AddScoped<ILocalReadProvider, LocalPostgresReadProvider>();
        services.AddScoped<ILocalWriteProvider, LocalPostgresWriteProvider>();

        // Registro de Validations
        services.AddValidatorsFromAssemblyContaining<CriarLocalValidation>();

        return services;
    }
}
```

## Checklist para Novos Módulos

### ✅ Estrutura de Pastas
- [ ] Criar estrutura de pastas seguindo o padrão
- [ ] Organizar arquivos nas camadas corretas
- [ ] Seguir nomenclatura padronizada

### ✅ Entidades
- [ ] Herdar de `EntidadeBase`
- [ ] Implementar métodos de criação, atualização e remoção
- [ ] Utilizar Value Objects quando apropriado
- [ ] Implementar regras de negócio

### ✅ DTOs
- [ ] Criar DTOs de entrada e saída
- [ ] Implementar validações com Data Annotations
- [ ] Criar métodos de conversão estáticos

### ✅ Use Cases
- [ ] Herdar de `BaseUseCase`
- [ ] Implementar interfaces correspondentes
- [ ] Executar validações antes de processar
- [ ] Utilizar providers para persistência

### ✅ Services
- [ ] Implementar interface do domínio
- [ ] Orquestrar casos de uso
- [ ] Converter entre DTOs e entidades

### ✅ Controllers
- [ ] Herdar de `ControllerRestBase`
- [ ] Implementar endpoints REST padrão
- [ ] Utilizar injeção de dependência
- [ ] Retornar respostas padronizadas

### ✅ Providers
- [ ] Implementar interfaces de leitura e escrita
- [ ] Configurar Entity Framework
- [ ] Implementar operações de CRUD

### ✅ Configurações
- [ ] Criar configuração de Entity Framework
- [ ] Configurar mapeamentos e constraints
- [ ] Definir índices para performance

### ✅ Validações
- [ ] Criar validações com FluentValidation
- [ ] Implementar specifications reutilizáveis
- [ ] Integrar validações nos use cases

### ✅ Testes
- [ ] Criar testes unitários para use cases
- [ ] Criar testes para validações
- [ ] Criar testes de integração

### ✅ Dependências
- [ ] Configurar package references
- [ ] Registrar dependências no DI container
- [ ] Configurar validações

## Conclusão

Esta arquitetura garante:
- **Separação de responsabilidades** clara entre camadas
- **Testabilidade** através de interfaces e injeção de dependência
- **Manutenibilidade** com código organizado e padronizado
- **Escalabilidade** com estrutura modular
- **Flexibilidade** para mudanças e extensões

Siga rigorosamente este padrão para manter consistência e qualidade em todos os módulos do sistema. 