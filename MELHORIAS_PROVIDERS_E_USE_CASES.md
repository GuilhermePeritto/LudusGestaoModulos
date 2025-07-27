# Melhorias nos Providers e Use Cases

## 📋 **Resumo das Melhorias Implementadas**

### 🏗️ **Arquitetura Base**

#### 1. **Interfaces Base Genéricas**
- **IReadProvider<TEntity>**: Interface base para todos os read providers
- **IWriteProvider<TEntity>**: Interface base para todos os write providers
- **Benefício**: Padronização e reutilização de código

#### 2. **Classes Base Abstratas**
- **ReadProviderBase<TEntity>**: Implementação base para read providers
- **WriteProviderBase<TEntity>**: Implementação base para write providers
- **Benefício**: Redução de código duplicado e consistência

#### 3. **Eliminação de Métodos Específicos por ID**
- **Antes**: Métodos `BuscarPorId()` nos providers
- **Depois**: Uso exclusivo de `QueryParamsHelper.BuscarPorId()`
- **Benefício**: Flexibilidade e consistência nas consultas

#### 4. **Correção da Lógica Redundante nos Use Cases**
- **Antes**: Use Cases de atualização recebiam ID e faziam validação redundante
- **Depois**: Use Cases recebem entidade já validada
- **Benefício**: Eliminação de validações duplicadas e responsabilidades claras

---

## 🔧 **Implementação Técnica**

### **1. Interfaces Base**

#### **IReadProvider<TEntity>**
```csharp
public interface IReadProvider<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> Listar();
    Task<IEnumerable<TEntity>> Listar(QueryParamsBase queryParams);
    Task<TEntity> Buscar(QueryParamsBase queryParams);
}
```

#### **IWriteProvider<TEntity>**
```csharp
public interface IWriteProvider<TEntity> where TEntity : class
{
    Task Adicionar(TEntity entity);
    Task Atualizar(TEntity entity);
    Task Remover(Guid id);
    Task<int> SalvarAlteracoes();
}
```

### **2. Classes Base**

#### **ReadProviderBase<TEntity>**
```csharp
public abstract class ReadProviderBase<TEntity> : IReadProvider<TEntity> where TEntity : class
{
    protected readonly DbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    protected ReadProviderBase(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TEntity>> Listar()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> Listar(QueryParamsBase queryParams)
    {
        var (query, _) = ApplyQueryParams(_dbSet.AsQueryable(), queryParams);
        return await query.ToListAsync();
    }

    public virtual async Task<TEntity> Buscar(QueryParamsBase queryParams)
    {
        var (query, _) = ApplyQueryParams(_dbSet.AsQueryable(), queryParams);
        return await query.FirstOrDefaultAsync();
    }

    protected abstract (IQueryable<TEntity> Query, int Total) ApplyQueryParams(IQueryable<TEntity> query, QueryParamsBase queryParams);
}
```

#### **WriteProviderBase<TEntity>**
```csharp
public abstract class WriteProviderBase<TEntity> : IWriteProvider<TEntity> where TEntity : class
{
    protected readonly DbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    protected WriteProviderBase(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public virtual async Task Adicionar(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public virtual async Task Atualizar(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public virtual async Task Remover(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
            _dbSet.Remove(entity);
    }

    public virtual async Task<int> SalvarAlteracoes()
    {
        return await _context.SaveChangesAsync();
    }
}
```

### **3. Correção da Lógica Redundante**

#### **Antes (Lógica Redundante)**
```csharp
// ❌ Use Case recebia ID e fazia validação redundante
public interface IAtualizarUsuarioUseCase
{
    Task<Usuario> Executar(Guid id, Usuario usuario);
}

public class AtualizarUsuarioUseCase : IAtualizarUsuarioUseCase
{
    private readonly IUsuarioReadProvider _readProvider;
    private readonly IUsuarioWriteProvider _writeProvider;

    public async Task<Usuario> Executar(Guid id, Usuario usuarioAtualizado)
    {
        // ❌ Validação redundante - já deveria receber entidade válida
        var queryParams = QueryParamsHelper.BuscarPorId(id);
        var usuarioExistente = await _readProvider.Buscar(queryParams);
        
        if (usuarioExistente == null)
        {
            Notificar("Usuário não encontrado.");
            return null;
        }

        // ❌ Lógica de atualização duplicada
        usuarioExistente.Atualizar(/* parâmetros */);
        await _writeProvider.Atualizar(usuarioExistente);
        return usuarioExistente;
    }
}
```

#### **Depois (Lógica Corrigida)**
```csharp
// ✅ Use Case recebe entidade já validada
public interface IAtualizarUsuarioUseCase
{
    Task<Usuario> Executar(Usuario usuario);
}

public class AtualizarUsuarioUseCase : IAtualizarUsuarioUseCase
{
    private readonly IUsuarioWriteProvider _writeProvider;

    public async Task<Usuario> Executar(Usuario usuario)
    {
        // ✅ Apenas validação de negócio
        if (!ExecutarValidacao(new AtualizarUsuarioValidation(), usuario))
            return null;

        // ✅ Operação direta
        await _writeProvider.Atualizar(usuario);
        await _writeProvider.SalvarAlteracoes();
        return usuario;
    }
}
```

### **4. Service Layer Atualizada**

#### **Antes (Service com Lógica Redundante)**
```csharp
public async Task<UsuarioDTO> Atualizar(Guid id, AtualizarUsuarioDTO dto)
{
    // ❌ Criação desnecessária de entidade temporária
    var usuario = Usuario.Criar(dto.Nome, dto.Email, /* ... */);
    var usuarioAtualizado = await _atualizarUseCase.Executar(id, usuario);
    
    return UsuarioDTO.Criar(usuarioAtualizado);
}
```

#### **Depois (Service com Lógica Correta)**
```csharp
public async Task<UsuarioDTO> Atualizar(Guid id, AtualizarUsuarioDTO dto)
{
    // ✅ Primeiro, buscar o usuário existente
    var usuarioExistente = await _buscarPorIdUseCase.Executar(id);
    if (usuarioExistente == null)
        return null;

    // ✅ Atualizar os dados do usuário existente
    usuarioExistente.Atualizar(dto.Nome, dto.Email, /* ... */);
    
    // ✅ Executar o use case de atualização
    var usuarioAtualizado = await _atualizarUseCase.Executar(usuarioExistente);
    
    return UsuarioDTO.Criar(usuarioAtualizado);
}
```

---

## 📊 **Benefícios das Melhorias**

### **1. Redução de Código**
- **Antes**: ~50 linhas por provider
- **Depois**: ~10 linhas por provider
- **Redução**: ~80% menos código

### **2. Consistência**
- **Padrão Único**: Todos os providers seguem o mesmo padrão
- **Interfaces Padronizadas**: Herança das interfaces base
- **Comportamento Previsível**: Métodos sempre funcionam da mesma forma

### **3. Flexibilidade**
- **QueryParamsHelper**: Filtros flexíveis e reutilizáveis
- **Extensibilidade**: Fácil adicionar novos filtros
- **Manutenibilidade**: Mudanças centralizadas

### **4. Eliminação de Redundância**
- **Validações Únicas**: Cada responsabilidade em seu lugar
- **Fluxo Claro**: Busca → Validação → Atualização
- **Performance**: Menos consultas desnecessárias

### **5. Testabilidade**
- **Interfaces Bem Definidas**: Fácil mockar para testes
- **Separação de Responsabilidades**: Cada classe tem uma função específica
- **Injeção de Dependência**: Dependências claras e testáveis

---

## 🔄 **Fluxo de Atualização Corrigido**

### **Antes (Fluxo Redundante)**
```
Controller → Service → AtualizarUseCase(id, dto) → BuscarPorId → Validar → Atualizar
```

### **Depois (Fluxo Otimizado)**
```
Controller → Service → BuscarPorIdUseCase(id) → AtualizarUseCase(entidade) → Atualizar
```

### **Vantagens do Novo Fluxo**
1. **Responsabilidades Claras**: Cada Use Case tem uma função específica
2. **Sem Redundância**: Validação de existência apenas uma vez
3. **Performance**: Menos consultas ao banco
4. **Manutenibilidade**: Lógica mais simples e clara

---

## 📝 **Padrões Estabelecidos**

### **1. Use Cases de Busca**
```csharp
public interface IBuscarPorIdUseCase<TEntity>
{
    Task<TEntity> Executar(Guid id);
}

public class BuscarPorIdUseCase<TEntity> : IBuscarPorIdUseCase<TEntity>
{
    public async Task<TEntity> Executar(Guid id)
    {
        var queryParams = QueryParamsHelper.BuscarPorId(id);
        var entity = await _readProvider.Buscar(queryParams);
        
        if (entity == null)
        {
            Notificar("Entidade não encontrada.");
            return null;
        }

        return entity;
    }
}
```

### **2. Use Cases de Atualização**
```csharp
public interface IAtualizarUseCase<TEntity>
{
    Task<TEntity> Executar(TEntity entity);
}

public class AtualizarUseCase<TEntity> : IAtualizarUseCase<TEntity>
{
    public async Task<TEntity> Executar(TEntity entity)
    {
        if (!ExecutarValidacao(new Validation(), entity))
            return null;

        await _writeProvider.Atualizar(entity);
        await _writeProvider.SalvarAlteracoes();
        return entity;
    }
}
```

### **3. Service Layer**
```csharp
public async Task<EntityDTO> Atualizar(Guid id, UpdateDTO dto)
{
    // 1. Buscar entidade existente
    var entity = await _buscarPorIdUseCase.Executar(id);
    if (entity == null)
        return null;

    // 2. Atualizar dados
    entity.Atualizar(dto.Propriedades);

    // 3. Executar use case de atualização
    var entityAtualizada = await _atualizarUseCase.Executar(entity);
    
    return EntityDTO.Criar(entityAtualizada);
}
```

---

## ✅ **Checklist de Implementação**

### **Arquivos Criados**
- [x] `IReadProvider<TEntity>` - Interface base para read providers
- [x] `IWriteProvider<TEntity>` - Interface base para write providers
- [x] `ReadProviderBase<TEntity>` - Classe base para read providers
- [x] `WriteProviderBase<TEntity>` - Classe base para write providers

### **Arquivos Atualizados - Módulo Usuario**
- [x] `IUsuarioReadProvider` - Herda de IReadProvider<Usuario>
- [x] `IUsuarioWriteProvider` - Herda de IWriteProvider<Usuario>
- [x] `UsuarioPostgresReadProvider` - Herda de ReadProviderBase<Usuario>
- [x] `UsuarioPostgresWriteProvider` - Herda de WriteProviderBase<Usuario>
- [x] `BuscarUsuarioPorIdUseCase` - Usa QueryParamsHelper
- [x] `AtualizarUsuarioUseCase` - Recebe entidade, não ID
- [x] `UsuarioService` - Fluxo corrigido

### **Arquivos Atualizados - Módulo Eventos**
- [x] `ILocalReadProvider` - Herda de IReadProvider<Local>
- [x] `ILocalWriteProvider` - Herda de IWriteProvider<Local>
- [x] `LocalPostgresReadProvider` - Herda de ReadProviderBase<Local>
- [x] `LocalPostgresWriteProvider` - Herda de WriteProviderBase<Local>
- [x] `BuscarLocalPorIdUseCase` - Usa QueryParamsHelper
- [x] `AtualizarLocalUseCase` - Recebe entidade, não ID
- [x] `LocalService` - Fluxo corrigido

### **Padrões Estabelecidos**
- [x] Nenhum método específico por ID nos providers
- [x] Uso exclusivo de QueryParamsHelper para filtros
- [x] Herança das interfaces e classes base
- [x] Separação clara entre read e write providers
- [x] Use Cases de atualização recebem entidade já validada
- [x] Eliminação de validações redundantes

---

## 🚀 **Próximos Passos**

### **1. Aplicar aos Outros Módulos**
- [ ] Módulo Empresa (já está correto)
- [ ] Módulo Filial (já está correto)
- [ ] Módulo Autenticação

### **2. Melhorias Futuras**
- [ ] Cache de queries complexas
- [ ] Paginação automática
- [ ] Filtros dinâmicos
- [ ] Auditoria de queries
- [ ] Validações automáticas baseadas em atributos

### **3. Documentação**
- [ ] Guia de criação de providers
- [ ] Exemplos de filtros complexos
- [ ] Padrões de nomenclatura
- [ ] Boas práticas

---

## 🎯 **Princípios Aplicados**

### **SOLID Principles**
- **SRP**: Cada Use Case tem uma responsabilidade específica
- **OCP**: Fácil extensão sem modificar código existente
- **LSP**: Interfaces base podem ser substituídas por implementações
- **ISP**: Interfaces específicas para cada necessidade
- **DIP**: Dependências de abstrações, não implementações

### **Clean Architecture**
- **Separação de Camadas**: Domain, Application, Infrastructure
- **Inversão de Dependência**: Providers implementam interfaces do Domain
- **Independência de Framework**: Lógica de negócio isolada
- **Testabilidade**: Fácil mockar dependências

---

Esta refatoração torna o sistema **muito mais consistente**, **flexível**, **eficiente** e **fácil de manter**, seguindo perfeitamente os princípios SOLID e Clean Architecture, eliminando redundâncias e estabelecendo responsabilidades claras. 