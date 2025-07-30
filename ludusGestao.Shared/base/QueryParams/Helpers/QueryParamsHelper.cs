using LudusGestao.Shared.Domain.QueryParams.Enums;

namespace LudusGestao.Shared.Domain.QueryParams.Helpers
{
    /// <summary>
    /// Helper melhorado para criar e manipular QueryParamsBase com métodos em português
    /// </summary>
    public static class QueryParamsHelper
    {
        #region Métodos de Criação de Filtros

        /// <summary>
        /// Cria um filtro simples
        /// </summary>
        public static QueryParamsBase CriarFiltro(string propriedade, object? valor, string operador = "eq")
        {
            var filtro = new QueryFilter 
            { 
                Property = propriedade, 
                Operator = operador, 
                Value = valor?.ToString() ?? string.Empty
            };
            
            return new QueryParamsBase 
            { 
                Filter = new List<QueryFilter> { filtro }
            };
        }

        /// <summary>
        /// Cria um filtro de igualdade
        /// </summary>
        public static QueryParamsBase CriarFiltroIgual(string propriedade, object? valor)
        {
            return CriarFiltro(propriedade, valor, "eq");
        }

        /// <summary>
        /// Cria um filtro de diferença
        /// </summary>
        public static QueryParamsBase CriarFiltroDiferente(string propriedade, object? valor)
        {
            return CriarFiltro(propriedade, valor, "neq");
        }

        /// <summary>
        /// Cria um filtro de maior que
        /// </summary>
        public static QueryParamsBase CriarFiltroMaiorQue(string propriedade, object? valor)
        {
            return CriarFiltro(propriedade, valor, "gt");
        }

        /// <summary>
        /// Cria um filtro de maior ou igual
        /// </summary>
        public static QueryParamsBase CriarFiltroMaiorOuIgual(string propriedade, object? valor)
        {
            return CriarFiltro(propriedade, valor, "gte");
        }

        /// <summary>
        /// Cria um filtro de menor que
        /// </summary>
        public static QueryParamsBase CriarFiltroMenorQue(string propriedade, object? valor)
        {
            return CriarFiltro(propriedade, valor, "lt");
        }

        /// <summary>
        /// Cria um filtro de menor ou igual
        /// </summary>
        public static QueryParamsBase CriarFiltroMenorOuIgual(string propriedade, object? valor)
        {
            return CriarFiltro(propriedade, valor, "lte");
        }

        /// <summary>
        /// Cria um filtro de contém
        /// </summary>
        public static QueryParamsBase CriarFiltroContem(string propriedade, string valor)
        {
            return CriarFiltro(propriedade, valor, "like");
        }

        /// <summary>
        /// Cria um filtro de começa com
        /// </summary>
        public static QueryParamsBase CriarFiltroComecaCom(string propriedade, string valor)
        {
            return CriarFiltro(propriedade, valor, "startswith");
        }

        /// <summary>
        /// Cria um filtro de termina com
        /// </summary>
        public static QueryParamsBase CriarFiltroTerminaCom(string propriedade, string valor)
        {
            return CriarFiltro(propriedade, valor, "endswith");
        }

        /// <summary>
        /// Cria um filtro de nulo
        /// </summary>
        public static QueryParamsBase CriarFiltroNulo(string propriedade)
        {
            return CriarFiltro(propriedade, null, "null");
        }

        /// <summary>
        /// Cria um filtro de não nulo
        /// </summary>
        public static QueryParamsBase CriarFiltroNaoNulo(string propriedade)
        {
            return CriarFiltro(propriedade, null, "notnull");
        }

        /// <summary>
        /// Cria múltiplos filtros
        /// </summary>
        public static QueryParamsBase CriarFiltrosMultiplos(params (string propriedade, object? valor, string operador)[] filtros)
        {
            var queryFilters = filtros.Select(f => new QueryFilter 
            { 
                Property = f.propriedade, 
                Operator = f.operador, 
                Value = f.valor?.ToString() ?? string.Empty
            }).ToList();
            
            return new QueryParamsBase 
            { 
                Filter = queryFilters
            };
        }

        #endregion

        #region Métodos de Filtro Específicos

        /// <summary>
        /// Filtra por ID
        /// </summary>
        public static QueryParamsBase FiltrarPorId(Guid id)
        {
            return CriarFiltroIgual("Id", id);
        }

        /// <summary>
        /// Filtra por nome usando LIKE
        /// </summary>
        public static QueryParamsBase FiltrarPorNome(string nome)
        {
            return CriarFiltroContem("Nome", nome);
        }

        /// <summary>
        /// Filtra por email usando LIKE
        /// </summary>
        public static QueryParamsBase FiltrarPorEmail(string email)
        {
            return CriarFiltroContem("Email", email);
        }

        /// <summary>
        /// Filtra por situação
        /// </summary>
        public static QueryParamsBase FiltrarPorSituacao(string situacao)
        {
            return CriarFiltroIgual("Situacao", situacao);
        }

        /// <summary>
        /// Filtra por situação ativa
        /// </summary>
        public static QueryParamsBase FiltrarPorAtivo()
        {
            return CriarFiltroIgual("Situacao", "Ativo");
        }

        /// <summary>
        /// Filtra por situação inativa
        /// </summary>
        public static QueryParamsBase FiltrarPorInativo()
        {
            return CriarFiltroIgual("Situacao", "Inativo");
        }

        /// <summary>
        /// Filtra por data de criação
        /// </summary>
        public static QueryParamsBase FiltrarPorDataCriacao(DateTime dataInicio, DateTime dataFim)
        {
            return CriarFiltrosMultiplos(
                ("DataCriacao", dataInicio, "gte"),
                ("DataCriacao", dataFim, "lte")
            );
        }

        /// <summary>
        /// Filtra por data de alteração
        /// </summary>
        public static QueryParamsBase FiltrarPorDataAlteracao(DateTime dataInicio, DateTime dataFim)
        {
            return CriarFiltrosMultiplos(
                ("DataAlteracao", dataInicio, "gte"),
                ("DataAlteracao", dataFim, "lte")
            );
        }

        /// <summary>
        /// Filtra por data de criação maior que
        /// </summary>
        public static QueryParamsBase FiltrarPorDataCriacaoMaiorQue(DateTime data)
        {
            return CriarFiltroMaiorQue("DataCriacao", data);
        }

        /// <summary>
        /// Filtra por data de criação menor que
        /// </summary>
        public static QueryParamsBase FiltrarPorDataCriacaoMenorQue(DateTime data)
        {
            return CriarFiltroMenorQue("DataCriacao", data);
        }

        #endregion

        #region Métodos de Filtro para ValueObjects

        /// <summary>
        /// Filtra por email (ValueObject)
        /// </summary>
        public static QueryParamsBase FiltrarPorEmailValueObject(string email)
        {
            return CriarFiltroContem("Email.Endereco", email);
        }

        /// <summary>
        /// Filtra por CNPJ (ValueObject)
        /// </summary>
        public static QueryParamsBase FiltrarPorCnpjValueObject(string cnpj)
        {
            return CriarFiltroContem("Cnpj.Numero", cnpj);
        }

        /// <summary>
        /// Filtra por CPF (ValueObject)
        /// </summary>
        public static QueryParamsBase FiltrarPorCpfValueObject(string cpf)
        {
            return CriarFiltroContem("Cpf.Numero", cpf);
        }

        /// <summary>
        /// Filtra por telefone (ValueObject)
        /// </summary>
        public static QueryParamsBase FiltrarPorTelefoneValueObject(string telefone)
        {
            return CriarFiltroContem("Telefone.Numero", telefone);
        }

        /// <summary>
        /// Filtra por cidade do endereço (ValueObject)
        /// </summary>
        public static QueryParamsBase FiltrarPorCidadeValueObject(string cidade)
        {
            return CriarFiltroContem("Endereco.Cidade", cidade);
        }

        /// <summary>
        /// Filtra por estado do endereço (ValueObject)
        /// </summary>
        public static QueryParamsBase FiltrarPorEstadoValueObject(string estado)
        {
            return CriarFiltroContem("Endereco.Estado", estado);
        }

        /// <summary>
        /// Filtra por CEP do endereço (ValueObject)
        /// </summary>
        public static QueryParamsBase FiltrarPorCepValueObject(string cep)
        {
            return CriarFiltroContem("Endereco.Cep", cep);
        }

        #endregion

        #region Métodos de Ordenação

        /// <summary>
        /// Ordena por nome (ascendente)
        /// </summary>
        public static QueryParamsBase OrdenarPorNome()
        {
            return new QueryParamsBase { Sort = "Nome" };
        }

        /// <summary>
        /// Ordena por nome (descendente)
        /// </summary>
        public static QueryParamsBase OrdenarPorNomeDescendente()
        {
            return new QueryParamsBase { Sort = "Nome desc" };
        }

        /// <summary>
        /// Ordena por data de criação (descendente)
        /// </summary>
        public static QueryParamsBase OrdenarPorDataCriacao()
        {
            return new QueryParamsBase { Sort = "DataCriacao desc" };
        }

        /// <summary>
        /// Ordena por data de criação (ascendente)
        /// </summary>
        public static QueryParamsBase OrdenarPorDataCriacaoAscendente()
        {
            return new QueryParamsBase { Sort = "DataCriacao" };
        }

        /// <summary>
        /// Ordena por data de alteração (descendente)
        /// </summary>
        public static QueryParamsBase OrdenarPorDataAlteracao()
        {
            return new QueryParamsBase { Sort = "DataAlteracao desc" };
        }

        /// <summary>
        /// Ordena por data de alteração (ascendente)
        /// </summary>
        public static QueryParamsBase OrdenarPorDataAlteracaoAscendente()
        {
            return new QueryParamsBase { Sort = "DataAlteracao" };
        }

        /// <summary>
        /// Cria ordenação customizada
        /// </summary>
        public static QueryParamsBase CriarOrdenacao(string campo, bool descendente = false)
        {
            var ordenacao = descendente ? $"{campo} desc" : campo;
            return new QueryParamsBase { Sort = ordenacao };
        }

        #endregion

        #region Métodos de Paginação

        /// <summary>
        /// Define a página
        /// </summary>
        public static QueryParamsBase DefinirPagina(int pagina)
        {
            return new QueryParamsBase { Page = pagina };
        }

        /// <summary>
        /// Define o limite de registros
        /// </summary>
        public static QueryParamsBase DefinirLimite(int limite)
        {
            return new QueryParamsBase { Limit = limite };
        }

        /// <summary>
        /// Define o offset
        /// </summary>
        public static QueryParamsBase DefinirOffset(int offset)
        {
            return new QueryParamsBase { Start = offset };
        }

        /// <summary>
        /// Cria paginação completa
        /// </summary>
        public static QueryParamsBase CriarPaginacao(int pagina, int limite)
        {
            return new QueryParamsBase 
            { 
                Page = pagina, 
                Limit = limite 
            };
        }

        /// <summary>
        /// Cria paginação com offset
        /// </summary>
        public static QueryParamsBase CriarPaginacaoComOffset(int offset, int limite)
        {
            return new QueryParamsBase 
            { 
                Start = offset, 
                Limit = limite 
            };
        }

        #endregion

        #region Métodos de Campos

        /// <summary>
        /// Define campos específicos
        /// </summary>
        public static QueryParamsBase DefinirCampos(params string[] campos)
        {
            return new QueryParamsBase { Fields = string.Join(",", campos) };
        }

        /// <summary>
        /// Define campos específicos como string
        /// </summary>
        public static QueryParamsBase DefinirCampos(string campos)
        {
            return new QueryParamsBase { Fields = campos };
        }

        /// <summary>
        /// Define campos básicos (Id, Nome)
        /// </summary>
        public static QueryParamsBase DefinirCamposBasicos()
        {
            return DefinirCampos("Id", "Nome");
        }

        /// <summary>
        /// Define campos de listagem (Id, Nome, Situacao)
        /// </summary>
        public static QueryParamsBase DefinirCamposListagem()
        {
            return DefinirCampos("Id", "Nome", "Situacao");
        }

        /// <summary>
        /// Define campos de detalhes (todos exceto campos sensíveis)
        /// </summary>
        public static QueryParamsBase DefinirCamposDetalhes()
        {
            return DefinirCampos("Id", "Nome", "Email", "Situacao", "DataCriacao", "DataAlteracao");
        }

        #endregion

        #region Métodos de Combinação

        /// <summary>
        /// Combina múltiplos QueryParams
        /// </summary>
        public static QueryParamsBase Combinar(params QueryParamsBase[] queryParams)
        {
            if (queryParams == null || queryParams.Length == 0)
                return new QueryParamsBase();

            var resultado = new QueryParamsBase();

            // Combina paginação (usa o primeiro não padrão)
            foreach (var param in queryParams)
            {
                if (param.Page > 1 || param.Start > 0 || param.Limit != 10)
                {
                    resultado.Page = param.Page;
                    resultado.Start = param.Start;
                    resultado.Limit = param.Limit;
                    break;
                }
            }

            // Combina ordenação (usa o primeiro não vazio)
            foreach (var param in queryParams)
            {
                if (!string.IsNullOrWhiteSpace(param.Sort))
                {
                    resultado.Sort = param.Sort;
                    break;
                }
            }

            // Combina campos
            var todosCampos = new List<string>();
            foreach (var param in queryParams)
            {
                if (!string.IsNullOrWhiteSpace(param.Fields))
                {
                    todosCampos.AddRange(param.Fields.Split(',', StringSplitOptions.RemoveEmptyEntries));
                }
            }

            if (todosCampos.Any())
            {
                resultado.Fields = string.Join(",", todosCampos.Distinct());
            }

            // Combina filtros
            var todosFiltros = new List<QueryFilter>();
            foreach (var param in queryParams)
            {
                if (param.Filter != null)
                    todosFiltros.AddRange(param.Filter);
            }

            if (todosFiltros.Any())
            {
                resultado.Filter = todosFiltros;
            }

            return resultado;
        }

        /// <summary>
        /// Combina filtro com ordenação
        /// </summary>
        public static QueryParamsBase CombinarFiltroEOrdenacao(QueryParamsBase filtro, QueryParamsBase ordenacao)
        {
            return Combinar(filtro, ordenacao);
        }

        /// <summary>
        /// Combina filtro com paginação
        /// </summary>
        public static QueryParamsBase CombinarFiltroEPaginacao(QueryParamsBase filtro, QueryParamsBase paginacao)
        {
            return Combinar(filtro, paginacao);
        }

        /// <summary>
        /// Combina filtro, ordenação e paginação
        /// </summary>
        public static QueryParamsBase CombinarFiltroOrdenacaoEPaginacao(QueryParamsBase filtro, QueryParamsBase ordenacao, QueryParamsBase paginacao)
        {
            return Combinar(filtro, ordenacao, paginacao);
        }

        #endregion

        #region Métodos de Validação

        /// <summary>
        /// Verifica se o QueryParams tem filtros
        /// </summary>
        public static bool PossuiFiltros(QueryParamsBase queryParams)
        {
            return queryParams.Filter != null && queryParams.Filter.Any();
        }

        /// <summary>
        /// Verifica se o QueryParams tem ordenação
        /// </summary>
        public static bool PossuiOrdenacao(QueryParamsBase queryParams)
        {
            return !string.IsNullOrWhiteSpace(queryParams.Sort);
        }

        /// <summary>
        /// Verifica se o QueryParams tem paginação
        /// </summary>
        public static bool PossuiPaginacao(QueryParamsBase queryParams)
        {
            return queryParams.Page > 1 || queryParams.Start > 0 || queryParams.Limit != 10;
        }

        /// <summary>
        /// Verifica se o QueryParams tem campos específicos definidos
        /// </summary>
        public static bool PossuiCampos(QueryParamsBase queryParams)
        {
            return !string.IsNullOrWhiteSpace(queryParams.Fields);
        }

        /// <summary>
        /// Obtém a lista de campos definidos no QueryParams
        /// </summary>
        public static List<string> ObterCampos(QueryParamsBase queryParams)
        {
            if (string.IsNullOrWhiteSpace(queryParams.Fields))
                return new List<string>();

            return queryParams.Fields
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.Trim())
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .ToList();
        }

        /// <summary>
        /// Obtém a quantidade de filtros
        /// </summary>
        public static int ObterQuantidadeFiltros(QueryParamsBase queryParams)
        {
            return queryParams.Filter?.Count ?? 0;
        }

        #endregion

        #region Métodos de Criação de Cenários Comuns

        /// <summary>
        /// Cria QueryParams para listagem básica
        /// </summary>
        public static QueryParamsBase CriarListagemBasica(int pagina = 1, int limite = 10)
        {
            return Combinar(
                CriarPaginacao(pagina, limite),
                OrdenarPorNome(),
                DefinirCamposListagem()
            );
        }

        /// <summary>
        /// Cria QueryParams para busca por nome
        /// </summary>
        public static QueryParamsBase CriarBuscaPorNome(string nome, int pagina = 1, int limite = 10)
        {
            return Combinar(
                FiltrarPorNome(nome),
                CriarPaginacao(pagina, limite),
                OrdenarPorNome()
            );
        }

        /// <summary>
        /// Cria QueryParams para listagem de ativos
        /// </summary>
        public static QueryParamsBase CriarListagemAtivos(int pagina = 1, int limite = 10)
        {
            return Combinar(
                FiltrarPorAtivo(),
                CriarPaginacao(pagina, limite),
                OrdenarPorNome()
            );
        }

        /// <summary>
        /// Cria QueryParams para detalhes completos
        /// </summary>
        public static QueryParamsBase CriarDetalhesCompletos()
        {
            return DefinirCamposDetalhes();
        }

        #endregion
    }
} 