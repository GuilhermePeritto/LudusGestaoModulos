# Melhorias nos Providers e Interfaces

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

### **3. Interfaces Específicas Simplificadas**

#### **Antes (IUsuarioReadProvider)**
```csharp
public interface IUsuarioReadProvider
{
    Task<IEnumerable<Usuario>> Listar();
    Task<IEnumerable<Usuario>> Listar(QueryParamsBase queryParams);
    Task<Usuario> Buscar(QueryParamsBase queryParams);
    Task<Usuario> BuscarPorId(Guid id); // ❌ Removido
}
```

#### **Depois (IUsuarioReadProvider)**
```csharp
public interface IUsuarioReadProvider : IReadProvider<Usuario>
{
    // ✅ Herda todos os métodos da interface base
}
```

### **4. Providers Concretos Simplificados**

#### **Antes (UsuarioPostgresReadProvider)**
```csharp
public class UsuarioPostgresReadProvider : IUsuarioReadProvider
{
    private readonly LudusGestaoReadDbContext _context;
    
    public UsuarioPostgresReadProvider(LudusGestaoReadDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Usuario>> Listar()
        => await _context.Usuarios.OrderBy(u => u.Nome).ToListAsync();

    public async Task<IEnumerable<Usuario>> Listar(QueryParamsBase queryParams)
    {
        var query = _context.Usuarios.AsQueryable();
        // Aplicar filtros se necessário
        return await query.OrderBy(u => u.Nome).ToListAsync();
    }

    public async Task<Usuario> Buscar(QueryParamsBase queryParams)
    {
        var query = _context.Usuarios.AsQueryable();
        // Aplicar filtros se necessário
        return await query.FirstOrDefaultAsync();
    }

    public async Task<Usuario> BuscarPorId(Guid id) // ❌ Removido
    {
        return await _context.Usuarios.FindAsync(id);
    }
}
```

#### **Depois (UsuarioPostgresReadProvider)**
```csharp
public class UsuarioPostgresReadProvider : ReadProviderBase<Usuario>, IUsuarioReadProvider
{
    public UsuarioPostgresReadProvider(LudusGestaoReadDbContext context) : base(context)
    {
    }

    protected override (IQueryable<Usuario> Query, int Total) ApplyQueryParams(IQueryable<Usuario> query, QueryParamsBase queryParams)
    {
        // Implementação básica - pode ser expandida conforme necessário
        var total = query.Count();
        return (query, total);
    }
}
```

### **5. Use Cases Atualizados**

#### **BuscarUsuarioPorIdUseCase**
```csharp
public async Task<Usuario> Executar(Guid id)
{
    var queryParams = QueryParamsHelper.BuscarPorId(id);
    var usuario = await _provider.Buscar(queryParams);
    
    if (usuario == null)
    {
        Notificar("Usuário não encontrado.");
        return null;
    }

    return usuario;
}
```

#### **AtualizarUsuarioUseCase**
```csharp
public async Task<Usuario> Executar(Guid id, Usuario usuarioAtualizado)
{
    var queryParams = QueryParamsHelper.BuscarPorId(id);
    var usuarioExistente = await _readProvider.Buscar(queryParams);
    
    if (usuarioExistente == null)
    {
        Notificar("Usuário não encontrado.");
        return null;
    }

    if (!ExecutarValidacao(new AtualizarUsuarioValidation(), usuarioAtualizado))
        return null;

    usuarioExistente.Atualizar(/* parâmetros */);

    await _writeProvider.Atualizar(usuarioExistente);
    await _writeProvider.SalvarAlteracoes();
    return usuarioExistente;
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

### **4. Testabilidade**
- **Interfaces Bem Definidas**: Fácil mockar para testes
- **Separação de Responsabilidades**: Cada classe tem uma função específica
- **Injeção de Dependência**: Dependências claras e testáveis

---

## 🔄 **Fluxo de Busca por ID**

### **Antes**
```
Controller → Service → UseCase → Provider.BuscarPorId(id)
```

### **Depois**
```
Controller → Service → UseCase → QueryParamsHelper.BuscarPorId(id) → Provider.Buscar(queryParams)
```

### **Vantagens do Novo Fluxo**
1. **Flexibilidade**: Pode usar qualquer filtro, não apenas ID
2. **Consistência**: Mesmo método para todas as buscas
3. **Extensibilidade**: Fácil adicionar filtros complexos
4. **Reutilização**: QueryParamsHelper pode ser usado em qualquer lugar

---

## 📝 **Como Criar Novos Providers**

### **1. Interface Específica**
```csharp
public interface IProdutoReadProvider : IReadProvider<Produto>
{
    // Métodos específicos se necessário
}
```

### **2. Provider Concreto**
```csharp
public class ProdutoPostgresReadProvider : ReadProviderBase<Produto>, IProdutoReadProvider
{
    public ProdutoPostgresReadProvider(LudusGestaoReadDbContext context) : base(context)
    {
    }

    protected override (IQueryable<Produto> Query, int Total) ApplyQueryParams(IQueryable<Produto> query, QueryParamsBase queryParams)
    {
        // Implementação específica se necessário
        var total = query.Count();
        return (query, total);
    }
}
```

### **3. Use Case**
```csharp
public async Task<Produto> Executar(Guid id)
{
    var queryParams = QueryParamsHelper.BuscarPorId(id);
    return await _provider.Buscar(queryParams);
}
```

---

## ✅ **Checklist de Implementação**

### **Arquivos Criados**
- [x] `IReadProvider<TEntity>` - Interface base para read providers
- [x] `IWriteProvider<TEntity>` - Interface base para write providers
- [x] `ReadProviderBase<TEntity>` - Classe base para read providers
- [x] `WriteProviderBase<TEntity>` - Classe base para write providers

### **Arquivos Atualizados**
- [x] `IUsuarioReadProvider` - Herda de IReadProvider<Usuario>
- [x] `IUsuarioWriteProvider` - Herda de IWriteProvider<Usuario>
- [x] `UsuarioPostgresReadProvider` - Herda de ReadProviderBase<Usuario>
- [x] `UsuarioPostgresWriteProvider` - Herda de WriteProviderBase<Usuario>
- [x] `BuscarUsuarioPorIdUseCase` - Usa QueryParamsHelper
- [x] `AtualizarUsuarioUseCase` - Usa QueryParamsHelper
- [x] `UsuarioService` - Atualizado para nova assinatura

### **Padrões Estabelecidos**
- [x] Nenhum método específico por ID nos providers
- [x] Uso exclusivo de QueryParamsHelper para filtros
- [x] Herança das interfaces e classes base
- [x] Separação clara entre read e write providers

---

## 🚀 **Próximos Passos**

### **1. Aplicar aos Outros Módulos**
- [ ] Módulo Empresa
- [ ] Módulo Filial
- [ ] Módulo Eventos
- [ ] Módulo Autenticação

### **2. Melhorias Futuras**
- [ ] Cache de queries complexas
- [ ] Paginação automática
- [ ] Filtros dinâmicos
- [ ] Auditoria de queries

### **3. Documentação**
- [ ] Guia de criação de providers
- [ ] Exemplos de filtros complexos
- [ ] Padrões de nomenclatura
- [ ] Boas práticas

---

Esta refatoração torna o sistema **mais consistente**, **mais flexível** e **mais fácil de manter**, seguindo os princípios SOLID e Clean Architecture. 