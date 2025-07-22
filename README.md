# Arquitetura da Aplicação

## Visão Geral

A aplicação segue uma arquitetura moderna, baseada em princípios de **Domain-Driven Design (DDD)**, **Clean Architecture** e **separação de responsabilidades**. O objetivo é garantir um sistema escalável, testável, de fácil manutenção e com regras de negócio centralizadas.

---

## Camadas e Responsabilidades

### 1. **API (Apresentação)**
- Responsável por receber as requisições HTTP, validar dados básicos e encaminhar para a camada de aplicação.
- Não contém lógica de negócio, apenas orquestra o fluxo e retorna respostas padronizadas.

### 2. **Aplicação**
- Orquestra o fluxo entre as camadas, coordenando casos de uso (operações de negócio).
- Centraliza validações, mapeamentos e chamadas a serviços de domínio e infraestrutura.
- Cada operação de negócio (criar, atualizar, remover, buscar) é tratada de forma explícita, facilitando a customização de regras.

### 3. **Domínio**
- Onde vivem as regras de negócio, entidades, especificações e validações de negócio.
- O domínio é isolado de detalhes de infraestrutura, focando apenas no que é importante para o negócio.
- As regras são expressas de forma clara, permitindo fácil evolução e testes.

### 4. **Infraestrutura (Provider)**
- Responsável pelo acesso a dados, integração com bancos, provedores externos e detalhes técnicos.
- Implementa repositórios e providers, abstraindo o acesso ao armazenamento.
- Permite trocar o mecanismo de persistência sem impactar o domínio ou a aplicação.

### 5. **Shared (Compartilhado)**
- Contém abstrações, contratos, padrões de resposta e utilitários comuns a todos os módulos.
- Garante consistência e reaproveitamento de código entre diferentes partes do sistema.

---

## Fluxo de uma Requisição

1. **Entrada na API:**  
   O usuário faz uma requisição HTTP para um endpoint.

2. **Validação Inicial:**  
   A API valida o formato dos dados (ex: campos obrigatórios, tipos).

3. **Encaminhamento para a Aplicação:**  
   A requisição é encaminhada para um caso de uso específico, que representa uma operação de negócio.

4. **Validação de Negócio:**  
   O caso de uso executa validações de negócio, usando regras e especificações do domínio.

5. **Mapeamento e Execução:**  
   Os dados são mapeados para entidades de domínio, que executam as operações necessárias (criação, atualização, etc).

6. **Persistência:**  
   O caso de uso solicita ao repositório/provider que salve ou recupere dados, sem conhecer detalhes do banco.

7. **Resposta Padronizada:**  
   O resultado é mapeado para um formato de resposta consistente e retornado à API, que envia ao usuário.

---

## Motivações e Benefícios

- **Separação de Responsabilidades:**  
  Cada camada tem um papel claro, evitando acoplamento e facilitando a manutenção.

- **Centralização das Regras de Negócio:**  
  Todas as regras importantes vivem no domínio, tornando o sistema mais confiável e fácil de evoluir.

- **Testabilidade:**  
  A arquitetura permite testar regras de negócio isoladamente, sem depender de banco ou infraestrutura.

- **Escalabilidade:**  
  Novos módulos, entidades e operações podem ser adicionados seguindo o mesmo padrão, sem duplicação de código.

- **Flexibilidade:**  
  Trocar banco de dados, adicionar cache, ou integrar com outros sistemas é simples, pois a infraestrutura é desacoplada do domínio.

- **Consistência:**  
  Padrões de resposta, validação e tratamento de erros são aplicados de forma uniforme em toda a aplicação.

---

## Estrutura de Pastas

### **ludusGestao.API/**
```
ludusGestao.API/
  Program.cs                    # Ponto de entrada da aplicação (configuração da API)
  appsettings.json              # Configurações gerais
  appsettings.Development.json  # Configurações para ambiente de desenvolvimento
  ludusGestao.API.http          # Exemplos de requisições HTTP para testes
  Properties/
  bin/
  obj/
  ludusGestao.API.csproj        # Projeto .NET
  ludusGestao.API.csproj.user
```

### **ludusGestao.Eventos/**
```
ludusGestao.Eventos/
  API/                          # Controllers específicos do módulo de eventos
  Application/                  # Casos de uso, serviços, validações e mapeadores do domínio de eventos
  Domain/                       # Entidades, DTOs, repositórios, especificações e providers do domínio de eventos
  bin/
  obj/
  ludusGestao.Eventos.csproj    # Projeto .NET
```

### **ludusGestao.Gerais/**
```
ludusGestao.Gerais/
  API/                          # Controllers do módulo de entidades gerais (empresa, filial, usuário)
  Application/                  # Casos de uso, serviços, validações e mapeadores do domínio geral
  Domain/                       # Entidades, DTOs, enums, repositórios, especificações e providers do domínio geral
  bin/
  obj/
  ludusGestao.Gerais.csproj     # Projeto .NET
```

### **ludusGestao.Provider/**
```
ludusGestao.Provider/
  Data/                         # Configurações de banco, contextos e providers de dados
  Extensions/                   # Extensões para injeção de dependências e configurações
  bin/
  obj/
  ludusGestao.Provider.csproj   # Projeto .NET
```

### **ludusGestao.Shared/**
```
ludusGestao.Shared/
  Application/                  # Abstrações, mapeadores, respostas e serviços compartilhados
  Domain/                       # Entidades base, value objects, repositórios e providers genéricos
  bin/
  obj/
  ludusGestao.Shared.csproj     # Projeto .NET
```

### **ludusGestao.Tests/**
```
ludusGestao.Tests/
  UnitTest1.cs                  # Exemplo de teste unitário
  bin/
  obj/
  ludusGestao.Tests.csproj      # Projeto de testes .NET
```

---

## Resumo Visual

```
Usuário → API → Aplicação (UseCase) → Domínio (Regras/Entidades) → Infraestrutura (Repositórios/Providers) → Banco/Serviços Externos
```

---

## Por que seguir esse modelo?

- **Facilita a manutenção e evolução do sistema.**
- **Permite crescimento seguro e organizado.**
- **Garante que o código reflete as regras do negócio, não detalhes técnicos.**
- **Reduz bugs e retrabalho, pois cada regra está em um único lugar.** 