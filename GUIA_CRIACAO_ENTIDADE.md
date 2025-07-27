# Guia Completo: Criação de Nova Entidade com CRUD

## 📋 **Visão Geral**

Este guia detalha o processo completo para criar uma nova entidade com operações CRUD (Create, Read, Update, Delete) seguindo a arquitetura DDD e Clean Architecture da aplicação.

---

## 🎯 **Exemplo: Criando a Entidade "Produto"**

Vamos usar como exemplo a criação de uma entidade `Produto` no módulo `Gerais`.

---

## 📁 **1. Estrutura de Pastas**

```
ludusGestao.Gerais/
├── Domain/
│   └── Entities/
│       └── Produto/
│           ├── DTOs/
│           │   ├── CriarProdutoDTO.cs
│           │   ├── AtualizarProdutoDTO.cs
│           │   └── ProdutoDTO.cs
│           ├── Interfaces/
│           │   ├── ICriarProdutoUseCase.cs
│           │   ├── IAtualizarProdutoUseCase.cs
│           │   ├── IRemoverProdutoUseCase.cs
│           │   ├── IBuscarProdutoPorIdUseCase.cs
│           │   ├── IListarProdutosUseCase.cs
│           │   ├── IProdutoReadProvider.cs
│           │   └── IProdutoWriteProvider.cs
│           ├── Specifications/
│           │   └── ProdutoAtivoSpecification.cs
│           ├── UseCases/
│           │   ├── CriarProdutoUseCase.cs
│           │   ├── AtualizarProdutoUseCase.cs
│           │   ├── RemoverProdutoUseCase.cs
│           │   ├── BuscarProdutoPorIdUseCase.cs
│           │   └── ListarProdutosUseCase.cs
│           ├── Validations/
│           │   ├── CriarProdutoValidation.cs
│           │   └── AtualizarProdutoValidation.cs
│           └── Produto.cs
├── Application/
│   └── Services/
│       └── ProdutoService.cs
└── API/
    └── Controllers/
        └── ProdutoController.cs
```

---

## 🏗️ **2. Criação da Entidade de Domínio**

### **2.1. Entidade Principal (Produto.cs)**

```csharp
using System;
using LudusGestao.Shared.Domain.Entities;
using LudusGestao.Shared.Domain.ValueObjects;

namespace ludusGestao.Gerais.Domain.Produto
{
    public enum SituacaoProduto
    {
        Ativo = SituacaoBase.Ativo,
        Inativo = SituacaoBase.Inativo
    }

    public class Produto : EntidadeBase
    {
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public decimal Preco { get; private set; }
        public string Codigo { get; private set; }
        public SituacaoProduto Situacao { get; private set; }
        public Guid CategoriaId { get; private set; }

        // Construtor privado para garantir criação via factory method
        private Produto() { }

        // Factory method para criação
        public static Produto Criar(string nome, string descricao, decimal preco, string codigo, Guid categoriaId)
        {
            return new Produto
            {
                Nome = nome,
                Descricao = descricao,
                Preco = preco,
                Codigo = codigo,
                CategoriaId = categoriaId,
                Situacao = SituacaoProduto.Ativo
            };
        }

        // Métodos de negócio
        public void Atualizar(string nome, string descricao, decimal preco, string codigo, Guid categoriaId)
        {
            Nome = nome;
            Descricao = descricao;
            Preco = preco;
            Codigo = codigo;
            CategoriaId = categoriaId;
            MarcarAlterado();
        }

        public void Ativar()
        {
            Situacao = SituacaoProduto.Ativo;
            MarcarAlterado();
        }

        public void Desativar()
        {
            Situacao = SituacaoProduto.Inativo;
            MarcarAlterado();
        }

        public bool EstaAtivo()
        {
            return Situacao == SituacaoProduto.Ativo;
        }

        public void AlterarPreco(decimal novoPreco)
        {
            if (novoPreco < 0)
                throw new ArgumentException("Preço não pode ser negativo");
            
            Preco = novoPreco;
            MarcarAlterado();
        }
    }
}
```

---

## 📝 **3. Criação dos DTOs**

### **3.1. DTO de Criação (CriarProdutoDTO.cs)**

```csharp
using System;
using System.ComponentModel.DataAnnotations;

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

        [Required(ErrorMessage = "O código é obrigatório.")]
        [StringLength(50, ErrorMessage = "O código deve ter no máximo 50 caracteres.")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "O ID da categoria é obrigatório.")]
        public Guid CategoriaId { get; set; }
    }
}
```

### **3.2. DTO de Atualização (AtualizarProdutoDTO.cs)**

```csharp
using System;
using System.ComponentModel.DataAnnotations;

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

        [Required(ErrorMessage = "O código é obrigatório.")]
        [StringLength(50, ErrorMessage = "O código deve ter no máximo 50 caracteres.")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "O ID da categoria é obrigatório.")]
        public Guid CategoriaId { get; set; }
    }
}
```

### **3.3. DTO de Resposta (ProdutoDTO.cs)**

```csharp
using System;

namespace ludusGestao.Gerais.Domain.Produto.DTOs
{
    public class ProdutoDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public string Codigo { get; set; }
        public bool Ativo { get; set; }
        public Guid CategoriaId { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }

        public static ProdutoDTO Criar(ludusGestao.Gerais.Domain.Produto.Produto produto)
        {
            return new ProdutoDTO
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                Codigo = produto.Codigo,
                Ativo = produto.EstaAtivo(),
                CategoriaId = produto.CategoriaId,
                DataCriacao = produto.DataCriacao,
                DataAlteracao = produto.DataAlteracao
            };
        }
    }
}
```

---

## 🔧 **4. Criação das Interfaces**

### **4.1. Interface do Read Provider (IProdutoReadProvider.cs)**

```csharp
using ludusGestao.Gerais.Domain.Produto;
using LudusGestao.Shared.Domain.Providers;

namespace ludusGestao.Gerais.Domain.Produto.Interfaces
{
    public interface IProdutoReadProvider : IReadProvider<Produto>
    {
    }
}
```

### **4.2. Interface do Write Provider (IProdutoWriteProvider.cs)**

```csharp
using ludusGestao.Gerais.Domain.Produto;
using LudusGestao.Shared.Domain.Providers;

namespace ludusGestao.Gerais.Domain.Produto.Interfaces
{
    public interface IProdutoWriteProvider : IWriteProvider<Produto>
    {
    }
}
```

### **4.3. Interfaces dos Use Cases**

```csharp
// ICriarProdutoUseCase.cs
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Produto;

namespace ludusGestao.Gerais.Domain.Produto.Interfaces
{
    public interface ICriarProdutoUseCase
    {
        Task<Produto> Executar(Produto produto);
    }
}

// IAtualizarProdutoUseCase.cs
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Produto;

namespace ludusGestao.Gerais.Domain.Produto.Interfaces
{
    public interface IAtualizarProdutoUseCase
    {
        Task<Produto> Executar(Produto produto);
    }
}

// IRemoverProdutoUseCase.cs
using System;
using System.Threading.Tasks;

namespace ludusGestao.Gerais.Domain.Produto.Interfaces
{
    public interface IRemoverProdutoUseCase
    {
        Task<bool> Executar(Guid id);
    }
}

// IBuscarProdutoPorIdUseCase.cs
using System;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Produto;

namespace ludusGestao.Gerais.Domain.Produto.Interfaces
{
    public interface IBuscarProdutoPorIdUseCase
    {
        Task<Produto> Executar(Guid id);
    }
}

// IListarProdutosUseCase.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;

namespace ludusGestao.Gerais.Domain.Produto.Interfaces
{
    public interface IListarProdutosUseCase
    {
        Task<IEnumerable<Produto>> Executar(QueryParamsBase queryParams = null);
    }
}
```

---

## ⚙️ **5. Implementação dos Use Cases**

### **5.1. Use Case de Criação (CriarProdutoUseCase.cs)**

```csharp
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Notificacao;
using ludusGestao.Gerais.Domain.Produto;
using ludusGestao.Gerais.Domain.Produto.Interfaces;
using ludusGestao.Gerais.Domain.Produto.Validations;

namespace ludusGestao.Gerais.Domain.Produto.UseCases
{
    public class CriarProdutoUseCase : BaseUseCase, ICriarProdutoUseCase
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

### **5.2. Use Case de Atualização (AtualizarProdutoUseCase.cs)**

```csharp
using System;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Notificacao;
using ludusGestao.Gerais.Domain.Produto;
using ludusGestao.Gerais.Domain.Produto.Interfaces;
using ludusGestao.Gerais.Domain.Produto.Validations;

namespace ludusGestao.Gerais.Domain.Produto.UseCases
{
    public class AtualizarProdutoUseCase : BaseUseCase, IAtualizarProdutoUseCase
    {
        private readonly IProdutoReadProvider _readProvider;
        private readonly IProdutoWriteProvider _writeProvider;

        public AtualizarProdutoUseCase(
            IProdutoReadProvider readProvider,
            IProdutoWriteProvider writeProvider,
            INotificador notificador)
            : base(notificador)
        {
            _readProvider = readProvider;
            _writeProvider = writeProvider;
        }

        public async Task<Produto> Executar(Guid id, Produto produtoAtualizado)
        {
            var produtoExistente = await _readProvider.BuscarPorId(id);
            if (produtoExistente == null)
            {
                Notificar("Produto não encontrado.");
                return null;
            }

            if (!ExecutarValidacao(new AtualizarProdutoValidation(), produtoAtualizado))
                return null;

            produtoExistente.Atualizar(
                produtoAtualizado.Nome,
                produtoAtualizado.Descricao,
                produtoAtualizado.Preco,
                produtoAtualizado.Codigo,
                produtoAtualizado.CategoriaId
            );

            await _writeProvider.Atualizar(produtoExistente);
            await _writeProvider.SalvarAlteracoes();
            return produtoExistente;
        }
    }
}
```

### **5.3. Use Case de Remoção (RemoverProdutoUseCase.cs)**

```csharp
using System;
using System.Threading.Tasks;
using LudusGestao.Shared.Notificacao;
using ludusGestao.Gerais.Domain.Produto.Interfaces;

namespace ludusGestao.Gerais.Domain.Produto.UseCases
{
    public class RemoverProdutoUseCase : IRemoverProdutoUseCase
    {
        private readonly IProdutoReadProvider _readProvider;
        private readonly IProdutoWriteProvider _writeProvider;
        private readonly INotificador _notificador;

        public RemoverProdutoUseCase(
            IProdutoReadProvider readProvider,
            IProdutoWriteProvider writeProvider,
            INotificador notificador)
        {
            _readProvider = readProvider;
            _writeProvider = writeProvider;
            _notificador = notificador;
        }

        public async Task<bool> Executar(Guid id)
        {
            var produto = await _readProvider.BuscarPorId(id);
            if (produto == null)
            {
                _notificador.Handle(new LudusGestao.Shared.Notificacao.Notificacao("Produto não encontrado."));
                return false;
            }

            await _writeProvider.Remover(id);
            await _writeProvider.SalvarAlteracoes();
            return true;
        }
    }
}
```

### **5.4. Use Case de Busca por ID (BuscarProdutoPorIdUseCase.cs)**

```csharp
using System;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Produto;
using ludusGestao.Gerais.Domain.Produto.Interfaces;

namespace ludusGestao.Gerais.Domain.Produto.UseCases
{
    public class BuscarProdutoPorIdUseCase : IBuscarProdutoPorIdUseCase
    {
        private readonly IProdutoReadProvider _provider;

        public BuscarProdutoPorIdUseCase(IProdutoReadProvider provider)
        {
            _provider = provider;
        }

        public async Task<Produto> Executar(Guid id)
        {
            return await _provider.BuscarPorId(id);
        }
    }
}
```

### **5.5. Use Case de Listagem (ListarProdutosUseCase.cs)**

```csharp
using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Gerais.Domain.Produto;
using ludusGestao.Gerais.Domain.Produto.Interfaces;

namespace ludusGestao.Gerais.Domain.Produto.UseCases
{
    public class ListarProdutosUseCase : IListarProdutosUseCase
    {
        private readonly IProdutoReadProvider _provider;

        public ListarProdutosUseCase(IProdutoReadProvider provider)
        {
            _provider = provider;
        }

        public async Task<IEnumerable<Produto>> Executar(QueryParamsBase queryParams = null)
        {
            if (queryParams == null)
                return await _provider.Listar();
            
            return await _provider.Listar(queryParams);
        }
    }
}
```

---

## ✅ **6. Criação das Validações**

### **6.1. Validação de Criação (CriarProdutoValidation.cs)**

```csharp
using FluentValidation;
using ludusGestao.Gerais.Domain.Produto;

namespace ludusGestao.Gerais.Domain.Produto.Validations
{
    public class CriarProdutoValidation : AbstractValidator<Produto>
    {
        public CriarProdutoValidation()
        {
            RuleFor(p => p.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MaximumLength(200).WithMessage("O nome deve ter no máximo 200 caracteres.");

            RuleFor(p => p.Preco)
                .GreaterThan(0).WithMessage("O preço deve ser maior que zero.");

            RuleFor(p => p.Codigo)
                .NotEmpty().WithMessage("O código é obrigatório.")
                .MaximumLength(50).WithMessage("O código deve ter no máximo 50 caracteres.");

            RuleFor(p => p.CategoriaId)
                .NotEmpty().WithMessage("A categoria é obrigatória.");
        }
    }
}
```

### **6.2. Validação de Atualização (AtualizarProdutoValidation.cs)**

```csharp
using FluentValidation;
using ludusGestao.Gerais.Domain.Produto;

namespace ludusGestao.Gerais.Domain.Produto.Validations
{
    public class AtualizarProdutoValidation : AbstractValidator<Produto>
    {
        public AtualizarProdutoValidation()
        {
            RuleFor(p => p.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MaximumLength(200).WithMessage("O nome deve ter no máximo 200 caracteres.");

            RuleFor(p => p.Preco)
                .GreaterThan(0).WithMessage("O preço deve ser maior que zero.");

            RuleFor(p => p.Codigo)
                .NotEmpty().WithMessage("O código é obrigatório.")
                .MaximumLength(50).WithMessage("O código deve ter no máximo 50 caracteres.");

            RuleFor(p => p.CategoriaId)
                .NotEmpty().WithMessage("A categoria é obrigatória.");
        }
    }
}
```

---

## 🏭 **7. Criação do Service**

### **7.1. Service (ProdutoService.cs)**

```csharp
using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Notificacao;
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

        public ProdutoService(
            ICriarProdutoUseCase criarUseCase,
            IAtualizarProdutoUseCase atualizarUseCase,
            IRemoverProdutoUseCase removerUseCase,
            IBuscarProdutoPorIdUseCase buscarPorIdUseCase,
            IListarProdutosUseCase listarUseCase,
            INotificador notificador)
            : base(notificador)
        {
            _criarUseCase = criarUseCase;
            _atualizarUseCase = atualizarUseCase;
            _removerUseCase = removerUseCase;
            _buscarPorIdUseCase = buscarPorIdUseCase;
            _listarUseCase = listarUseCase;
        }

        public async Task<ProdutoDTO> Criar(CriarProdutoDTO dto)
        {
            var produto = Produto.Criar(dto.Nome, dto.Descricao, dto.Preco, dto.Codigo, dto.CategoriaId);
            var produtoCriado = await _criarUseCase.Executar(produto);
            
            if (produtoCriado == null)
                return null;

            return ProdutoDTO.Criar(produtoCriado);
        }

        public async Task<ProdutoDTO> Atualizar(Guid id, AtualizarProdutoDTO dto)
        {
            var produto = Produto.Criar(dto.Nome, dto.Descricao, dto.Preco, dto.Codigo, dto.CategoriaId);
            var produtoAtualizado = await _atualizarUseCase.Executar(id, produto);
            
            if (produtoAtualizado == null)
                return null;

            return ProdutoDTO.Criar(produtoAtualizado);
        }

        public async Task<bool> Remover(Guid id)
        {
            return await _removerUseCase.Executar(id);
        }

        public async Task<ProdutoDTO> BuscarPorId(Guid id)
        {
            var produto = await _buscarPorIdUseCase.Executar(id);
            return produto != null ? ProdutoDTO.Criar(produto) : null;
        }

        public async Task<IEnumerable<ProdutoDTO>> Listar(QueryParamsBase queryParams = null)
        {
            var produtos = await _listarUseCase.Executar(queryParams);
            var produtosDTO = new List<ProdutoDTO>();
            
            foreach (var produto in produtos)
            {
                produtosDTO.Add(ProdutoDTO.Criar(produto));
            }
            
            return produtosDTO;
        }
    }
}
```

### **7.2. Interface do Service (IProdutoService.cs)**

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
        Task<IEnumerable<ProdutoDTO>> Listar(QueryParamsBase queryParams = null);
    }
}
```

---

## 🎮 **8. Criação do Controller**

### **8.1. Controller (ProdutoController.cs)**

```csharp
using System;
using System.Net;
using LudusGestao.Shared.Domain.Controllers;
using Microsoft.AspNetCore.Mvc;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Notificacao;
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
                return CustomResponse(HttpStatusCode.BadRequest, "Erro ao atualizar produto.");

            return CustomResponse(HttpStatusCode.OK, result, "Produto atualizado com sucesso.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(Guid id)
        {
            var result = await _service.Remover(id);

            if (!result)
                return CustomResponse(HttpStatusCode.BadRequest, "Erro ao remover produto.");

            return CustomResponse(HttpStatusCode.OK, "Produto removido com sucesso.");
        }
    }
}
```

---

## 🗄️ **9. Configuração do Banco de Dados**

### **9.1. Configuration (ProdutoConfiguration.cs)**

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

            builder.Property(p => p.Codigo)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.Situacao)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(p => p.CategoriaId)
                .IsRequired();

            // Índices para performance
            builder.HasIndex(p => p.Codigo).IsUnique();
            builder.HasIndex(p => p.CategoriaId);
            builder.HasIndex(p => p.Situacao);
        }
    }
}
```

### **9.2. Atualizar DbContexts**

Adicionar o DbSet nos contextos:

```csharp
// LudusGestaoReadDbContext.cs e LudusGestaoWriteDbContext.cs
public DbSet<Produto> Produtos { get; set; }

// No método ApplyTenantFilters, adicionar:
ApplyTenantFilter<Produto>(modelBuilder);
```

---

## 🔌 **10. Implementação dos Providers**

### **10.1. Read Provider (ProdutoPostgresReadProvider.cs)**

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ludusGestao.Gerais.Domain.Produto.Interfaces;
using ludusGestao.Provider.Data.Contexts;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Domain.Providers;

namespace ludusGestao.Provider.Data.Providers.Gerais.ProdutoProvider
{
    public class ProdutoPostgresReadProvider : ProviderBase<ludusGestao.Gerais.Domain.Produto.Produto>, IProdutoReadProvider
    {
        private readonly LudusGestaoReadDbContext _context;

        public ProdutoPostgresReadProvider(LudusGestaoReadDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ludusGestao.Gerais.Domain.Produto.Produto>> Listar()
            => await _context.Produtos.OrderBy(p => p.Nome).ToListAsync();

        public async Task<IEnumerable<ludusGestao.Gerais.Domain.Produto.Produto>> Listar(QueryParamsBase queryParams)
        {
            var (query, _) = ApplyQueryParams(_context.Produtos.AsQueryable(), queryParams);
            return await query.OrderBy(p => p.Nome).ToListAsync();
        }

        public async Task<ludusGestao.Gerais.Domain.Produto.Produto> Buscar(QueryParamsBase queryParams)
        {
            var (query, _) = ApplyQueryParams(_context.Produtos.AsQueryable(), queryParams);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<ludusGestao.Gerais.Domain.Produto.Produto> BuscarPorId(Guid id)
        {
            return await _context.Produtos.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
```

### **10.2. Write Provider (ProdutoPostgresWriteProvider.cs)**

```csharp
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ludusGestao.Gerais.Domain.Produto.Interfaces;
using ludusGestao.Provider.Data.Contexts;

namespace ludusGestao.Provider.Data.Providers.Gerais.ProdutoProvider
{
    public class ProdutoPostgresWriteProvider : IProdutoWriteProvider
    {
        private readonly LudusGestaoWriteDbContext _context;

        public ProdutoPostgresWriteProvider(LudusGestaoWriteDbContext context)
        {
            _context = context;
        }

        public async Task Adicionar(ludusGestao.Gerais.Domain.Produto.Produto produto)
        {
            await _context.Produtos.AddAsync(produto);
        }

        public async Task Atualizar(ludusGestao.Gerais.Domain.Produto.Produto produto)
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

---

## 🔧 **11. Configuração da Injeção de Dependência**

### **11.1. Atualizar ServiceCollectionExtensions.cs**

```csharp
// Adicionar no método AddGeraisModule()

// Registro de Use Cases - Produto
services.AddScoped<ICriarProdutoUseCase, CriarProdutoUseCase>();
services.AddScoped<IAtualizarProdutoUseCase, AtualizarProdutoUseCase>();
services.AddScoped<IRemoverProdutoUseCase, RemoverProdutoUseCase>();
services.AddScoped<IBuscarProdutoPorIdUseCase, BuscarProdutoPorIdUseCase>();
services.AddScoped<IListarProdutosUseCase, ListarProdutosUseCase>();

// Registro de Services - Produto
services.AddScoped<IProdutoService, ProdutoService>();

// Registro de Validations - Produto
services.AddScoped<CriarProdutoValidation>();
services.AddScoped<AtualizarProdutoValidation>();
```

### **11.2. Atualizar ProviderSelector.cs**

```csharp
// Adicionar no método ConfigureServices()

// Providers - Produto
services.AddScoped<IProdutoReadProvider, ProdutoPostgresReadProvider>();
services.AddScoped<IProdutoWriteProvider, ProdutoPostgresWriteProvider>();
```

---

## 📋 **12. Checklist de Implementação**

### **✅ Arquivos Criados**
- [ ] `Produto.cs` - Entidade de domínio
- [ ] `CriarProdutoDTO.cs` - DTO de criação
- [ ] `AtualizarProdutoDTO.cs` - DTO de atualização
- [ ] `ProdutoDTO.cs` - DTO de resposta
- [ ] `IProdutoReadProvider.cs` - Interface do read provider
- [ ] `IProdutoWriteProvider.cs` - Interface do write provider
- [ ] `ICriarProdutoUseCase.cs` - Interface do use case de criação
- [ ] `IAtualizarProdutoUseCase.cs` - Interface do use case de atualização
- [ ] `IRemoverProdutoUseCase.cs` - Interface do use case de remoção
- [ ] `IBuscarProdutoPorIdUseCase.cs` - Interface do use case de busca
- [ ] `IListarProdutosUseCase.cs` - Interface do use case de listagem
- [ ] `CriarProdutoUseCase.cs` - Use case de criação
- [ ] `AtualizarProdutoUseCase.cs` - Use case de atualização
- [ ] `RemoverProdutoUseCase.cs` - Use case de remoção
- [ ] `BuscarProdutoPorIdUseCase.cs` - Use case de busca
- [ ] `ListarProdutosUseCase.cs` - Use case de listagem
- [ ] `CriarProdutoValidation.cs` - Validação de criação
- [ ] `AtualizarProdutoValidation.cs` - Validação de atualização
- [ ] `IProdutoService.cs` - Interface do service
- [ ] `ProdutoService.cs` - Service
- [ ] `ProdutoController.cs` - Controller
- [ ] `ProdutoConfiguration.cs` - Configuração do Entity Framework
- [ ] `ProdutoPostgresReadProvider.cs` - Read provider
- [ ] `ProdutoPostgresWriteProvider.cs` - Write provider

### **✅ Configurações Atualizadas**
- [ ] DbContexts com DbSet
- [ ] ServiceCollectionExtensions com registros
- [ ] ProviderSelector com providers
- [ ] TenantFilterBuilder com filtro

### **✅ Testes**
- [ ] Build do projeto
- [ ] Teste de criação
- [ ] Teste de listagem
- [ ] Teste de busca por ID
- [ ] Teste de atualização
- [ ] Teste de remoção
- [ ] Teste de validações
- [ ] Teste de multitenant

---

## 🚀 **13. Comandos para Testar**

```bash
# Build do projeto
dotnet build

# Executar a aplicação
dotnet run --project ludusGestao.API

# Testar endpoints (usando curl ou Postman)
curl -X POST "https://localhost:7001/api/produtos" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer SEU_JWT_TOKEN_AQUI" \
  -d '{
    "nome": "Produto Teste",
    "descricao": "Descrição do produto",
    "preco": 99.99,
    "codigo": "PROD001",
    "categoriaId": "00000000-0000-0000-0000-000000000001"
  }'

curl -X GET "https://localhost:7001/api/produtos" \
  -H "Authorization: Bearer SEU_JWT_TOKEN_AQUI"
```

---

## 📚 **14. Padrões Importantes**

### **14.1. Nomenclatura**
- **Entidades**: PascalCase (ex: `Produto`)
- **DTOs**: PascalCase + DTO (ex: `CriarProdutoDTO`)
- **Use Cases**: PascalCase + UseCase (ex: `CriarProdutoUseCase`)
- **Providers**: PascalCase + Provider (ex: `ProdutoPostgresReadProvider`)
- **Controllers**: PascalCase + Controller (ex: `ProdutoController`)

### **14.2. Estrutura de Pastas**
- **Domain**: Regras de negócio e entidades
- **Application**: Orquestração e serviços
- **API**: Controllers e endpoints
- **Provider**: Acesso a dados

### **14.3. Princípios**
- **Single Responsibility**: Cada classe tem uma responsabilidade
- **Dependency Inversion**: Dependências através de interfaces
- **Open/Closed**: Extensível sem modificação
- **Liskov Substitution**: Implementações intercambiáveis

---

Este guia fornece um roteiro completo para criar qualquer entidade seguindo a arquitetura estabelecida. Cada passo é detalhado e inclui exemplos práticos para facilitar a implementação. 