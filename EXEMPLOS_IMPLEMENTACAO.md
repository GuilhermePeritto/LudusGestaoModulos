# Exemplos Práticos de Implementação - Padrão Módulo Eventos

Este documento contém exemplos práticos de como implementar cada componente seguindo o padrão estabelecido pelo módulo de eventos.

## 1. Implementação de Entidade

### Exemplo: Entidade Produto

```csharp
using System;
using LudusGestao.Shared.Domain.Entities;
using LudusGestao.Shared.Domain.ValueObjects;

namespace ludusGestao.Gerais.Domain.Produto
{
    public class Produto : EntidadeBase
    {
        public Produto() { }
        
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public decimal Preco { get; private set; }
        public int QuantidadeEstoque { get; private set; }
        public bool Ativo { get; private set; }
        public int TenantId { get; private set; }

        public static Produto Criar(string nome, string descricao, decimal preco, int quantidadeEstoque)
        {
            return new Produto
            {
                Nome = nome,
                Descricao = descricao,
                Preco = preco,
                QuantidadeEstoque = quantidadeEstoque,
                Ativo = true
            };
        }

        public void Atualizar(string nome, string descricao, decimal preco, int quantidadeEstoque)
        {
            Nome = nome;
            Descricao = descricao;
            Preco = preco;
            QuantidadeEstoque = quantidadeEstoque;
        }

        public void AtualizarEstoque(int quantidade)
        {
            if (quantidade < 0 && Math.Abs(quantidade) > QuantidadeEstoque)
                throw new InvalidOperationException("Quantidade insuficiente em estoque.");
            
            QuantidadeEstoque += quantidade;
        }

        public void Remover()
        {
            Ativo = false;
        }

        public void Reativar()
        {
            Ativo = true;
        }

        public void AlterarTenant(int tenantId)
        {
            TenantId = tenantId;
        }
    }
}
```

## 2. Implementação de DTOs

### DTO de Resposta
```csharp
using System;
using ludusGestao.Gerais.Domain.Produto;

namespace ludusGestao.Gerais.Domain.Produto.DTOs
{
    public class ProdutoDTO
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public decimal Preco { get; private set; }
        public int QuantidadeEstoque { get; private set; }
        public bool Ativo { get; private set; }
        public int TenantId { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime? DataAlteracao { get; private set; }

        public static ProdutoDTO Criar(Produto produto)
        {
            return new ProdutoDTO
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                QuantidadeEstoque = produto.QuantidadeEstoque,
                Ativo = produto.Ativo,
                TenantId = produto.TenantId,
                DataCriacao = produto.DataCriacao,
                DataAlteracao = produto.DataAlteracao
            };
        }
    }
}
```

### DTO de Entrada para Criação
```csharp
using System.ComponentModel.DataAnnotations;
using ludusGestao.Gerais.Domain.Produto;

namespace ludusGestao.Gerais.Domain.Produto.DTOs
{
    public class CriarProdutoDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres.")]
        public string Nome { get; set; }

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "A quantidade em estoque é obrigatória.")]
        [Range(0, int.MaxValue, ErrorMessage = "A quantidade em estoque deve ser maior ou igual a zero.")]
        public int QuantidadeEstoque { get; set; }
    }
}
```

### DTO de Entrada para Atualização
```csharp
using System.ComponentModel.DataAnnotations;
using ludusGestao.Gerais.Domain.Produto;

namespace ludusGestao.Gerais.Domain.Produto.DTOs
{
    public class AtualizarProdutoDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres.")]
        public string Nome { get; set; }

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "A quantidade em estoque é obrigatória.")]
        [Range(0, int.MaxValue, ErrorMessage = "A quantidade em estoque deve ser maior ou igual a zero.")]
        public int QuantidadeEstoque { get; set; }
    }
}
```

## 3. Implementação de Interfaces

### Interface do Service
```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Gerais.Domain.Produto.DTOs;

namespace ludusGestao.Gerais.Domain.Produto.Interfaces
{
    public interface IProdutoService
    {
        Task<ProdutoDTO> Criar(CriarProdutoDTO dto);
        Task<ProdutoDTO> Atualizar(Guid id, AtualizarProdutoDTO dto);
        Task<bool> Remover(Guid id);
        Task<ProdutoDTO> BuscarPorId(Guid id);
        Task<IEnumerable<ProdutoDTO>> Listar(QueryParamsBase query);
        Task<ProdutoDTO> AtualizarEstoque(Guid id, int quantidade);
    }
}
```

### Interfaces dos Use Cases
```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Gerais.Domain.Produto;

namespace ludusGestao.Gerais.Domain.Produto.Interfaces
{
    public interface ICriarProdutoUseCase
    {
        Task<Produto> Executar(Produto produto);
    }

    public interface IAtualizarProdutoUseCase
    {
        Task<Produto> Executar(Produto produto);
    }

    public interface IRemoverProdutoUseCase
    {
        Task<bool> Executar(Produto produto);
    }

    public interface IBuscarProdutoPorIdUseCase
    {
        Task<Produto> Executar(Guid id);
    }

    public interface IListarProdutosUseCase
    {
        Task<IEnumerable<Produto>> Executar(QueryParamsBase query);
    }

    public interface IAtualizarEstoqueProdutoUseCase
    {
        Task<Produto> Executar(Guid id, int quantidade);
    }
}
```

### Interfaces dos Providers
```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Gerais.Domain.Produto;

namespace ludusGestao.Gerais.Domain.Produto.Interfaces
{
    public interface IProdutoReadProvider : IReadProvider<Produto>
    {
        Task<IEnumerable<Produto>> BuscarPorNome(string nome);
        Task<IEnumerable<Produto>> BuscarPorFaixaPreco(decimal precoMinimo, decimal precoMaximo);
    }

    public interface IProdutoWriteProvider
    {
        Task Adicionar(Produto produto);
        Task Atualizar(Produto produto);
        Task Remover(Guid id);
        Task<int> SalvarAlteracoes();
    }
}
```

## 4. Implementação de Use Cases

### Use Case de Criação
```csharp
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Gerais.Domain.Produto;
using ludusGestao.Gerais.Domain.Produto.Interfaces;
using ludusGestao.Gerais.Domain.Produto.Validations;

namespace ludusGestao.Gerais.Domain.Produto.UseCases
{
    public class CriarProdutoUseCase : BaseUseCase
    {
        private readonly IProdutoWriteProvider _provider;

        public CriarProdutoUseCase(IProdutoWriteProvider provider, INotificador notificador)
            : base(notificador)
        {
            _provider = provider;
        }

        public async Task<Produto> Executar(Produto produto)
        {
            if (!ExecutarValidacao(new CriarProdutoValidation(), produto))
                return null;

            await _provider.Adicionar(produto);
            await _provider.SalvarAlteracoes();
            return produto;
        }
    }
}
```

### Use Case de Atualização
```csharp
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Gerais.Domain.Produto;
using ludusGestao.Gerais.Domain.Produto.Interfaces;
using ludusGestao.Gerais.Domain.Produto.Validations;

namespace ludusGestao.Gerais.Domain.Produto.UseCases
{
    public class AtualizarProdutoUseCase : BaseUseCase
    {
        private readonly IProdutoWriteProvider _provider;

        public AtualizarProdutoUseCase(IProdutoWriteProvider provider, INotificador notificador)
            : base(notificador)
        {
            _provider = provider;
        }

        public async Task<Produto> Executar(Produto produto)
        {
            if (!ExecutarValidacao(new AtualizarProdutoValidation(), produto))
                return null;

            await _provider.Atualizar(produto);
            await _provider.SalvarAlteracoes();
            return produto;
        }
    }
}
```

### Use Case de Busca por ID
```csharp
using System;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Gerais.Domain.Produto;
using ludusGestao.Gerais.Domain.Produto.Interfaces;

namespace ludusGestao.Gerais.Domain.Produto.UseCases
{
    public class BuscarProdutoPorIdUseCase : BaseUseCase
    {
        private readonly IProdutoReadProvider _provider;

        public BuscarProdutoPorIdUseCase(IProdutoReadProvider provider, INotificador notificador)
            : base(notificador)
        {
            _provider = provider;
        }

        public async Task<Produto> Executar(Guid id)
        {
            var produto = await _provider.BuscarPorId(id);
            
            if (produto == null)
            {
                Notificar("Produto não encontrado.");
                return null;
            }

            return produto;
        }
    }
}
```

## 5. Implementação de Specifications

```csharp
using ludusGestao.Gerais.Domain.Produto;

namespace ludusGestao.Gerais.Domain.Produto.Specifications
{
    public class ProdutoAtivoSpecification
    {
        public string MensagemErro => "O produto precisa estar ativo para ser atualizado.";
        
        public bool IsSatisfiedBy(Produto produto)
        {
            return produto.Ativo;
        }
    }

    public class ProdutoComEstoqueSpecification
    {
        public string MensagemErro => "O produto deve ter estoque disponível.";
        
        public bool IsSatisfiedBy(Produto produto)
        {
            return produto.QuantidadeEstoque > 0;
        }
    }

    public class ProdutoPrecoValidoSpecification
    {
        public string MensagemErro => "O preço do produto deve ser maior que zero.";
        
        public bool IsSatisfiedBy(Produto produto)
        {
            return produto.Preco > 0;
        }
    }
}
```

## 6. Implementação de Validations

```csharp
using FluentValidation;
using ludusGestao.Gerais.Domain.Produto;
using ludusGestao.Gerais.Domain.Produto.Specifications;

namespace ludusGestao.Gerais.Domain.Produto.Validations
{
    public class CriarProdutoValidation : AbstractValidator<Produto>
    {
        public CriarProdutoValidation()
        {
            RuleFor(p => p.Nome)
                .NotEmpty().WithMessage("O nome do produto é obrigatório.")
                .MaximumLength(200).WithMessage("O nome deve ter no máximo 200 caracteres.");

            RuleFor(p => p.Preco)
                .GreaterThan(0).WithMessage("O preço deve ser maior que zero.");

            RuleFor(p => p.QuantidadeEstoque)
                .GreaterThanOrEqualTo(0).WithMessage("A quantidade em estoque deve ser maior ou igual a zero.");

            RuleFor(p => p)
                .Must(p => new ProdutoAtivoSpecification().IsSatisfiedBy(p))
                .WithMessage("O produto precisa estar ativo para ser criado.");

            RuleFor(p => p)
                .Must(p => new ProdutoPrecoValidoSpecification().IsSatisfiedBy(p))
                .WithMessage("O preço do produto deve ser válido.");
        }
    }

    public class AtualizarProdutoValidation : AbstractValidator<Produto>
    {
        public AtualizarProdutoValidation()
        {
            RuleFor(p => p.Nome)
                .NotEmpty().WithMessage("O nome do produto é obrigatório.")
                .MaximumLength(200).WithMessage("O nome deve ter no máximo 200 caracteres.");

            RuleFor(p => p.Preco)
                .GreaterThan(0).WithMessage("O preço deve ser maior que zero.");

            RuleFor(p => p.QuantidadeEstoque)
                .GreaterThanOrEqualTo(0).WithMessage("A quantidade em estoque deve ser maior ou igual a zero.");

            RuleFor(p => p)
                .Must(p => new ProdutoAtivoSpecification().IsSatisfiedBy(p))
                .WithMessage("O produto precisa estar ativo para ser atualizado.");
        }
    }
}
```

## 7. Implementação do Service

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Gerais.Domain.Produto.DTOs;
using ludusGestao.Gerais.Domain.Produto.Interfaces;
using ludusGestao.Gerais.Domain.Produto;

namespace ludusGestao.Gerais.Application.Services
{
    public class ProdutoService : BaseService, IProdutoService
    {
        private readonly ICriarProdutoUseCase _criarUseCase;
        private readonly IAtualizarProdutoUseCase _atualizarUseCase;
        private readonly IRemoverProdutoUseCase _removerUseCase;
        private readonly IBuscarProdutoPorIdUseCase _buscarPorIdUseCase;
        private readonly IListarProdutosUseCase _listarUseCase;
        private readonly IAtualizarEstoqueProdutoUseCase _atualizarEstoqueUseCase;

        public ProdutoService(
            ICriarProdutoUseCase criarUseCase,
            IAtualizarProdutoUseCase atualizarUseCase,
            IRemoverProdutoUseCase removerUseCase,
            IBuscarProdutoPorIdUseCase buscarPorIdUseCase,
            IListarProdutosUseCase listarUseCase,
            IAtualizarEstoqueProdutoUseCase atualizarEstoqueUseCase,
            INotificador notificador)
            : base(notificador)
        {
            _criarUseCase = criarUseCase;
            _atualizarUseCase = atualizarUseCase;
            _removerUseCase = removerUseCase;
            _buscarPorIdUseCase = buscarPorIdUseCase;
            _listarUseCase = listarUseCase;
            _atualizarEstoqueUseCase = atualizarEstoqueUseCase;
        }

        public async Task<ProdutoDTO> Criar(CriarProdutoDTO dto)
        {
            var produto = Produto.Criar(dto.Nome, dto.Descricao, dto.Preco, dto.QuantidadeEstoque);
            var produtoCriado = await _criarUseCase.Executar(produto);
            
            if (produtoCriado == null)
                return null;

            return ProdutoDTO.Criar(produtoCriado);
        }

        public async Task<ProdutoDTO> Atualizar(Guid id, AtualizarProdutoDTO dto)
        {
            var produto = await _buscarPorIdUseCase.Executar(id);
            
            if (produto == null)
                return null;

            produto.Atualizar(dto.Nome, dto.Descricao, dto.Preco, dto.QuantidadeEstoque);
            var produtoAtualizado = await _atualizarUseCase.Executar(produto);
            
            if (produtoAtualizado == null)
                return null;

            return ProdutoDTO.Criar(produtoAtualizado);
        }

        public async Task<bool> Remover(Guid id)
        {
            var produto = await _buscarPorIdUseCase.Executar(id);
            
            if (produto == null)
                return false;

            return await _removerUseCase.Executar(produto);
        }

        public async Task<ProdutoDTO> BuscarPorId(Guid id)
        {
            var produto = await _buscarPorIdUseCase.Executar(id);
            
            if (produto == null)
                return null;

            return ProdutoDTO.Criar(produto);
        }

        public async Task<IEnumerable<ProdutoDTO>> Listar(QueryParamsBase query)
        {
            var produtos = await _listarUseCase.Executar(query);
            return produtos.Select(ProdutoDTO.Criar);
        }

        public async Task<ProdutoDTO> AtualizarEstoque(Guid id, int quantidade)
        {
            var produto = await _buscarPorIdUseCase.Executar(id);
            
            if (produto == null)
                return null;

            produto.AtualizarEstoque(quantidade);
            var produtoAtualizado = await _atualizarUseCase.Executar(produto);
            
            if (produtoAtualizado == null)
                return null;

            return ProdutoDTO.Criar(produtoAtualizado);
        }
    }
}
```

## 8. Implementação do Controller

```csharp
using System;
using System.Net;
using LudusGestao.Shared.Application.Controllers;
using Microsoft.AspNetCore.Mvc;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Gerais.Domain.Produto.DTOs;
using ludusGestao.Gerais.Domain.Produto.Interfaces;

namespace ludusGestao.Gerais.API.Controllers
{
    [ApiController]
    [Route("api/produtos")]
    public class ProdutoController : ControllerRestBase
    {
        private readonly IProdutoService _service;

        public ProdutoController(INotificador notificador, IProdutoService service)
            : base(notificador)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] QueryParamsBase query)
        {
            var result = await _service.Listar(query);
            return CustomResponse(HttpStatusCode.OK, result, "Produtos listados com sucesso.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var result = await _service.BuscarPorId(id);

            if (result == null)
                return CustomResponse(HttpStatusCode.NotFound, $"Produto com código {id} não encontrado");

            return CustomResponse(HttpStatusCode.OK, result, "Produto encontrado com sucesso.");
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarProdutoDTO dto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var result = await _service.Criar(dto);

            if (result == null)
                return CustomResponse(HttpStatusCode.BadRequest, "Erro ao criar produto.");

            return CustomResponse(HttpStatusCode.Created, result, "Produto criado com sucesso.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarProdutoDTO dto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var result = await _service.Atualizar(id, dto);

            if (result == null)
                return CustomResponse(HttpStatusCode.NotFound, "Produto não encontrado.");

            return CustomResponse(HttpStatusCode.NoContent, result, "Produto atualizado com sucesso.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(Guid id)
        {
            var result = await _service.Remover(id);

            if (!result)
                return CustomResponse(HttpStatusCode.NotFound, "Produto não encontrado.");

            return CustomResponse(HttpStatusCode.NoContent, result, "Produto removido com sucesso.");
        }

        [HttpPatch("{id}/estoque")]
        public async Task<IActionResult> AtualizarEstoque(Guid id, [FromBody] int quantidade)
        {
            var result = await _service.AtualizarEstoque(id, quantidade);

            if (result == null)
                return CustomResponse(HttpStatusCode.NotFound, "Produto não encontrado.");

            return CustomResponse(HttpStatusCode.OK, result, "Estoque atualizado com sucesso.");
        }
    }
}
```

## 9. Implementação dos Providers

### Read Provider
```csharp
using Microsoft.EntityFrameworkCore;
using ludusGestao.Provider.Data.Contexts;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Domain.Providers;
using ludusGestao.Gerais.Domain.Produto;
using ludusGestao.Gerais.Domain.Produto.Interfaces;

namespace ludusGestao.Provider.Data.Providers.Gerais.Produto
{
    public class ProdutoPostgresReadProvider : ProviderBase<Produto>, IProdutoReadProvider
    {
        public ProdutoPostgresReadProvider(LudusGestaoReadDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Produto>> BuscarPorNome(string nome)
        {
            return await _context.Produtos
                .Where(p => p.Nome.Contains(nome) && p.Ativo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Produto>> BuscarPorFaixaPreco(decimal precoMinimo, decimal precoMaximo)
        {
            return await _context.Produtos
                .Where(p => p.Preco >= precoMinimo && p.Preco <= precoMaximo && p.Ativo)
                .ToListAsync();
        }
    }
}
```

### Write Provider
```csharp
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProdutoEntity = ludusGestao.Gerais.Domain.Produto;
using ludusGestao.Gerais.Domain.Produto.Interfaces;
using ludusGestao.Provider.Data.Contexts;

namespace ludusGestao.Provider.Data.Providers.Gerais.Produto
{
    public class ProdutoPostgresWriteProvider : IProdutoWriteProvider
    {
        private readonly LudusGestaoWriteDbContext _context;

        public ProdutoPostgresWriteProvider(LudusGestaoWriteDbContext context)
        {
            _context = context;
        }

        public async Task Adicionar(ProdutoEntity produto)
        {
            await _context.Produtos.AddAsync(produto);
        }

        public async Task Atualizar(ProdutoEntity produto)
        {
            _context.Produtos.Update(produto);
        }

        public async Task Remover(Guid id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto != null)
                _context.Produtos.Remove(produto);
        }

        public async Task<int> SalvarAlteracoes()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
```

## 10. Configuração do Entity Framework

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ludusGestao.Gerais.Domain.Produto;

namespace ludusGestao.Provider.Data.Configurations.Gerais
{
    public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("Produtos");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Descricao)
                .HasMaxLength(500);

            builder.Property(p => p.Preco)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.QuantidadeEstoque)
                .IsRequired();

            builder.Property(p => p.Ativo)
                .IsRequired()
                .HasDefaultValue(true);

            // Índices para melhor performance
            builder.HasIndex(p => p.Nome);
            builder.HasIndex(p => p.Preco);
            builder.HasIndex(p => p.Ativo);

            // Configurações de auditoria
            builder.Property(p => p.DataCriacao)
                .IsRequired();

            builder.Property(p => p.DataAlteracao);
        }
    }
}
```

## 11. Configuração de Dependências

```csharp
using Microsoft.Extensions.DependencyInjection;
using ludusGestao.Gerais.Domain.Produto.Interfaces;
using ludusGestao.Gerais.Domain.Produto.UseCases;
using ludusGestao.Gerais.Application.Services;
using ludusGestao.Gerais.Domain.Produto.Validations;
using ludusGestao.Provider.Data.Providers.Gerais.Produto;

namespace ludusGestao.Gerais.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGeraisModule(this IServiceCollection services)
        {
            // Registro de Use Cases
            services.AddScoped<ICriarProdutoUseCase, CriarProdutoUseCase>();
            services.AddScoped<IAtualizarProdutoUseCase, AtualizarProdutoUseCase>();
            services.AddScoped<IRemoverProdutoUseCase, RemoverProdutoUseCase>();
            services.AddScoped<IBuscarProdutoPorIdUseCase, BuscarProdutoPorIdUseCase>();
            services.AddScoped<IListarProdutosUseCase, ListarProdutosUseCase>();
            services.AddScoped<IAtualizarEstoqueProdutoUseCase, AtualizarEstoqueProdutoUseCase>();

            // Registro de Services
            services.AddScoped<IProdutoService, ProdutoService>();

            // Registro de Providers
            services.AddScoped<IProdutoReadProvider, ProdutoPostgresReadProvider>();
            services.AddScoped<IProdutoWriteProvider, ProdutoPostgresWriteProvider>();

            // Registro de Validations
            services.AddValidatorsFromAssemblyContaining<CriarProdutoValidation>();

            return services;
        }
    }
}
```

## Conclusão

Estes exemplos demonstram como implementar cada componente seguindo rigorosamente o padrão estabelecido pelo módulo de eventos. Mantenha a consistência na nomenclatura, estrutura e implementação para garantir a qualidade e manutenibilidade do código. 