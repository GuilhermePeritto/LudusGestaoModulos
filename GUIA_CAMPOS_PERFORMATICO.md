# 🚀 Guia de Uso - Campos Performáticos (Fields) - SELECT Dinâmico

## 🎯 Visão Geral

A funcionalidade de **campos performáticos** implementa SELECT dinâmico no banco de dados, retornando apenas os campos solicitados diretamente do banco. Isso garante máxima performance e otimização de tráfego de dados.

## ⚡ **Principais Benefícios**

- ✅ **SELECT dinâmico**: Apenas os campos solicitados são buscados no banco
- ✅ **Máxima performance**: Menos dados trafegados e processados
- ✅ **Flexibilidade**: Cliente escolhe exatamente o que precisa
- ✅ **Compatibilidade**: Funciona com filtros, ordenação e paginação
- ✅ **DynamicEntityDTO**: Retorno flexível e tipado

## 🔧 **Como Funciona**

### **1. Processamento no Banco de Dados**

```sql
-- Quando fields=nome,email é especificado:
SELECT Nome, Email FROM Usuarios WHERE Situacao = 'Ativo' ORDER BY Nome

-- Em vez de:
SELECT Id, Nome, Email, Telefone, Situacao, DataCriacao, DataAlteracao FROM Usuarios WHERE Situacao = 'Ativo' ORDER BY Nome
```

### **2. DynamicEntityDTO**

O sistema retorna um DTO dinâmico que contém apenas os campos solicitados:

```json
[
  {
    "nome": "João Silva",
    "email": "joao@email.com"
  },
  {
    "nome": "Maria Santos", 
    "email": "maria@email.com"
  }
]
```

## 📝 **Exemplos de Uso**

### **Exemplo 1: Máxima Performance**

```http
GET /api/usuarios?fields=id,nome&limit=100
```

**Resultado:**
```json
[
  { "id": "123", "nome": "João Silva" },
  { "id": "124", "nome": "Maria Santos" }
]
```

### **Exemplo 2: Dados Essenciais**

```http
GET /api/usuarios?fields=nome,email,situacao&filter=[{"property":"situacao","operator":"eq","value":"Ativo"}]
```

### **Exemplo 3: Lista Otimizada**

```http
GET /api/empresas?fields=nome,cnpj&sort=nome&page=1&limit=20
```

## 🛠️ **Implementação Técnica**

### **1. Fluxo de Execução**

```
Request → Controller → UseCase → Provider → SELECT Dinâmico → DynamicEntityDTO → Response
```

### **2. Componentes Principais**

#### **QueryFilterProcessor**
```csharp
// Processa campos solicitados
var fields = QueryFilterProcessor.ProcessFields<TEntity>(queryParams);

// Aplica SELECT dinâmico
var dynamicQuery = QueryFilterProcessor.ApplyFieldsFilterAsDTO(query, fields);
```

#### **DynamicEntityDTO**
```csharp
// Cria DTO com campos específicos
var dto = DynamicEntityDTO.FromEntity(entity, fields);

// Converte para Dictionary
var dict = dto.ToDictionary();
```

#### **ProviderBase/ReadProviderBase**
```csharp
// Métodos tradicionais (retornam entidades completas)
var entities = await provider.Listar(queryParams);
var entity = await provider.Buscar(queryParams);

// Métodos com campos específicos (retornam DynamicEntityDTO)
var entitiesWithFields = await provider.ListarComCampos(queryParams);
var entityWithFields = await provider.BuscarComCampos(queryParams);
```

### **3. Como Usar nos Controllers**

```csharp
[HttpGet]
public async Task<IActionResult> Listar([FromQuery] QueryParamsBase queryParams)
{
    // Verifica se há campos específicos solicitados
    var fields = QueryFilterProcessor.ProcessFields<Usuario>(queryParams);
    
    if (fields.Any())
    {
        // Usa método específico para campos
        var result = await _usuarioService.ListarComCampos(queryParams);
        return Ok(new RespostaBase(result));
    }
    else
    {
        // Usa método tradicional
        var result = await _usuarioService.Listar(queryParams);
        return Ok(new RespostaBase(result));
    }
}
```

## 📊 **Comparação de Performance**

### **Antes (Sem Fields)**
```sql
SELECT Id, Nome, Email, Telefone, Situacao, DataCriacao, DataAlteracao 
FROM Usuarios 
WHERE Situacao = 'Ativo' 
ORDER BY Nome 
LIMIT 50
```

### **Depois (Com Fields)**
```sql
SELECT Nome, Email 
FROM Usuarios 
WHERE Situacao = 'Ativo' 
ORDER BY Nome 
LIMIT 50
```

**Benefícios:**
- 🚀 **60-80% menos dados** trafegados
- ⚡ **Melhor performance** de rede
- 💾 **Menos uso de memória**
- 🔄 **Processamento mais rápido**

## 🎨 **Casos de Uso Comuns**

### **1. Listas Simples (Máxima Performance)**
```http
GET /api/usuarios?fields=id,nome&limit=100
```

### **2. Dados de Contato**
```http
GET /api/usuarios?fields=nome,email,telefone
```

### **3. Dados de Status**
```http
GET /api/usuarios?fields=nome,situacao,dataCriacao
```

### **4. Dados Completos (Padrão)**
```http
GET /api/usuarios
```

## 🔍 **Validação e Segurança**

### **Campos Válidos**
- ✅ Campos existentes na entidade são retornados
- ✅ Case insensitive (nome, Nome, NOME funcionam)
- ✅ Campos inválidos são ignorados silenciosamente

### **Exemplo de Validação**
```http
GET /api/usuarios?fields=nome,campoInexistente,email
```

**Resultado:** Apenas `nome` e `email` são retornados, `campoInexistente` é ignorado.

## 🚀 **Dicas de Performance**

### **1. Use campos específicos para listas grandes**
```http
GET /api/usuarios?fields=id,nome&limit=1000
```

### **2. Combine com filtros para consultas otimizadas**
```http
GET /api/usuarios?fields=nome,email&filter=[{"property":"situacao","operator":"eq","value":"Ativo"}]&limit=50
```

### **3. Use paginação para grandes volumes**
```http
GET /api/usuarios?fields=id,nome&page=1&limit=100
```

### **4. Teste diferentes combinações**
```http
# Para dropdowns
GET /api/usuarios?fields=id,nome&sort=nome

# Para grids
GET /api/usuarios?fields=id,nome,email,situacao&sort=nome&page=1&limit=20
```

## 🔗 **Integração com Outras Funcionalidades**

- ✅ **Filtros**: Funciona perfeitamente com filtros
- ✅ **Ordenação**: Compatível com ordenação
- ✅ **Paginação**: Funciona com paginação
- ✅ **Tenant**: Compatível com multi-tenancy
- ✅ **Auditoria**: Mantém campos de auditoria quando necessário

## 📋 **Propriedades Suportadas**

### **Entidade Usuario:**
- `id`, `nome`, `email`, `telefone`, `situacao`, `dataCriacao`, `dataAlteracao`

### **Entidade Empresa:**
- `id`, `nome`, `cnpj`, `situacao`, `dataCriacao`, `dataAlteracao`

### **Entidade Filial:**
- `id`, `nome`, `empresaId`, `situacao`, `dataCriacao`, `dataAlteracao`

## 🧪 **Testes**

Execute os testes para verificar a funcionalidade:

```bash
dotnet test --filter "DynamicFieldsTest"
```

## 📈 **Monitoramento**

### **Métricas de Performance**
- Tempo de resposta da API
- Volume de dados trafegados
- Uso de memória
- Performance do banco de dados

### **Logs**
- Campos solicitados
- Campos válidos processados
- Campos inválidos ignorados

## 🎯 **Próximos Passos**

1. **Teste em produção** com diferentes cenários
2. **Monitore performance** e ajuste conforme necessário
3. **Documente casos de uso** específicos do seu domínio
4. **Considere cache** para consultas frequentes
5. **Implemente métricas** de uso da funcionalidade 