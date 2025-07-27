using LudusGestao.Shared.Domain.Common;

namespace LudusGestao.Shared.Domain.Providers
{
    public static class QueryParamsHelper
    {
        #region Métodos Base

        /// <summary>
        /// Cria um filtro básico com propriedade, valor e operador
        /// </summary>
        public static QueryParamsBase Filtrar(string propriedade, object valor, string operador = "eq")
        {
            return new QueryParamsBase 
            { 
                FilterObjects = new List<FilterObject>
                { 
                    new FilterObject 
                    { 
                        Property = propriedade, 
                        Value = valor, 
                        Operator = operador 
                    } 
                } 
            };
        }

        /// <summary>
        /// Cria um filtro usando a sintaxe simples (propriedade:valor)
        /// </summary>
        public static QueryParamsBase FiltrarSimples(string propriedade, object valor)
        {
            return new QueryParamsBase { Filter = $"{propriedade}:{valor}" };
        }

        /// <summary>
        /// Define ordenação por propriedade
        /// </summary>
        public static QueryParamsBase OrdenarPor(string propriedade)
        {
            return new QueryParamsBase { Sort = propriedade };
        }

        /// <summary>
        /// Define paginação
        /// </summary>
        public static QueryParamsBase Paginar(int pagina = 1, int limite = 10)
        {
            return new QueryParamsBase 
            { 
                Page = pagina,
                Limit = limite
            };
        }

        #endregion

        #region Filtros Específicos

        /// <summary>
        /// Filtra por ID (Guid)
        /// </summary>
        public static QueryParamsBase FiltrarPorId(Guid id)
        {
            return Filtrar("Id", id, "eq");
        }

        /// <summary>
        /// Filtra por ID (string)
        /// </summary>
        public static QueryParamsBase FiltrarPorId(string id)
        {
            return Filtrar("Id", id, "eq");
        }

        /// <summary>
        /// Filtra por nome (usando operador 'like' por padrão)
        /// </summary>
        public static QueryParamsBase FiltrarPorNome(string nome, string operador = "like")
        {
            return Filtrar("Nome", nome, operador);
        }

        /// <summary>
        /// Filtra por status ativo
        /// </summary>
        public static QueryParamsBase FiltrarPorAtivo(bool ativo)
        {
            return Filtrar("Ativo", ativo, "eq");
        }

        /// <summary>
        /// Filtra por tenant ID
        /// </summary>
        public static QueryParamsBase FiltrarPorTenant(int tenantId)
        {
            return Filtrar("TenantId", tenantId, "eq");
        }

        /// <summary>
        /// Filtra por email
        /// </summary>
        public static QueryParamsBase FiltrarPorEmail(string email, string operador = "eq")
        {
            return Filtrar("Email", email, operador);
        }

        /// <summary>
        /// Filtra por CNPJ
        /// </summary>
        public static QueryParamsBase FiltrarPorCnpj(string cnpj, string operador = "eq")
        {
            return Filtrar("Cnpj", cnpj, operador);
        }

        /// <summary>
        /// Filtra por CPF
        /// </summary>
        public static QueryParamsBase FiltrarPorCpf(string cpf, string operador = "eq")
        {
            return Filtrar("Cpf", cpf, operador);
        }

        #endregion

        #region Ordenações Específicas

        /// <summary>
        /// Ordena por nome
        /// </summary>
        public static QueryParamsBase OrdenarPorNome()
        {
            return OrdenarPor("Nome");
        }

        /// <summary>
        /// Ordena por ID
        /// </summary>
        public static QueryParamsBase OrdenarPorId()
        {
            return OrdenarPor("Id");
        }

        /// <summary>
        /// Ordena por data de criação (mais recente primeiro)
        /// </summary>
        public static QueryParamsBase OrdenarPorDataCriacao()
        {
            return OrdenarPor("DataCriacao desc");
        }

        /// <summary>
        /// Ordena por data de alteração (mais recente primeiro)
        /// </summary>
        public static QueryParamsBase OrdenarPorDataAlteracao()
        {
            return OrdenarPor("DataAlteracao desc");
        }

        #endregion

        #region Combinações Comuns

        /// <summary>
        /// Combina filtro e ordenação
        /// </summary>
        public static QueryParamsBase FiltrarEOrdenar(string propriedade, object valor, string ordenacao, string operador = "eq")
        {
            return Combinar(
                Filtrar(propriedade, valor, operador),
                OrdenarPor(ordenacao)
            );
        }

        /// <summary>
        /// Combina filtro, ordenação e paginação
        /// </summary>
        public static QueryParamsBase FiltrarOrdenarEPaginar(string propriedade, object valor, string ordenacao, int pagina = 1, int limite = 10, string operador = "eq")
        {
            return Combinar(
                Filtrar(propriedade, valor, operador),
                OrdenarPor(ordenacao),
                Paginar(pagina, limite)
            );
        }

        /// <summary>
        /// Lista com paginação padrão (página 1, limite 10)
        /// </summary>
        public static QueryParamsBase ListarComPaginacao(int pagina = 1, int limite = 10)
        {
            return Paginar(pagina, limite);
        }

        /// <summary>
        /// Lista com ordenação e paginação
        /// </summary>
        public static QueryParamsBase ListarComOrdenacao(string propriedadeOrdenacao, int pagina = 1, int limite = 10)
        {
            return Combinar(
                OrdenarPor(propriedadeOrdenacao),
                Paginar(pagina, limite)
            );
        }

        /// <summary>
        /// Lista com filtro e paginação
        /// </summary>
        public static QueryParamsBase ListarComFiltro(string propriedade, object valor, int pagina = 1, int limite = 10, string operador = "eq")
        {
            return Combinar(
                Filtrar(propriedade, valor, operador),
                Paginar(pagina, limite)
            );
        }

        /// <summary>
        /// Lista completa com filtro, ordenação e paginação
        /// </summary>
        public static QueryParamsBase ListarCompleto(string propriedade, object valor, string ordenacao, int pagina = 1, int limite = 10, string operador = "eq")
        {
            return Combinar(
                Filtrar(propriedade, valor, operador),
                OrdenarPor(ordenacao),
                Paginar(pagina, limite)
            );
        }

        #endregion

        #region Utilitários

        /// <summary>
        /// Retorna uma query vazia (sem filtros, ordenação ou paginação)
        /// </summary>
        public static QueryParamsBase ListarTodos()
        {
            return new QueryParamsBase();
        }

        /// <summary>
        /// Combina múltiplas queries em uma única
        /// </summary>
        public static QueryParamsBase Combinar(params QueryParamsBase[] queries)
        {
            if (queries == null || queries.Length == 0)
                return new QueryParamsBase();

            var resultado = new QueryParamsBase();
            var todosFiltros = new List<FilterObject>();

            foreach (var query in queries)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    // Converte filtro simples para FilterObjects
                    var parts = query.Filter.Split(':', 2);
                    if (parts.Length == 2)
                    {
                        todosFiltros.Add(new FilterObject 
                        { 
                            Property = parts[0], 
                            Value = parts[1], 
                            Operator = "eq" 
                        });
                    }
                }

                if (query.FilterObjects != null)
                {
                    todosFiltros.AddRange(query.FilterObjects);
                }

                if (!string.IsNullOrEmpty(query.Sort))
                    resultado.Sort = query.Sort;

                if (query.Page > 0)
                    resultado.Page = query.Page;

                if (query.Limit > 0)
                    resultado.Limit = query.Limit;
            }

            if (todosFiltros.Any())
                resultado.FilterObjects = todosFiltros;

            return resultado;
        }

        #endregion

        #region Métodos de Compatibilidade (Deprecated)

        /// <summary>
        /// Método de compatibilidade - use FiltrarPorId(Guid) em vez disso
        /// </summary>
        [Obsolete("Use FiltrarPorId(Guid) em vez disso")]
        public static QueryParamsBase BuscarPorId(Guid id)
        {
            return FiltrarPorId(id);
        }

        /// <summary>
        /// Método de compatibilidade - use FiltrarPorId(string) em vez disso
        /// </summary>
        [Obsolete("Use FiltrarPorId(string) em vez disso")]
        public static QueryParamsBase BuscarPorId(string id)
        {
            return FiltrarPorId(id);
        }

        /// <summary>
        /// Método de compatibilidade - use FiltrarSimples em vez disso
        /// </summary>
        [Obsolete("Use FiltrarSimples em vez disso")]
        public static QueryParamsBase BuscarPorPropriedade(string propriedade, string valor)
        {
            return FiltrarSimples(propriedade, valor);
        }

        /// <summary>
        /// Método de compatibilidade - use FiltrarSimples em vez disso
        /// </summary>
        [Obsolete("Use FiltrarSimples em vez disso")]
        public static QueryParamsBase BuscarPorPropriedade(string propriedade, int valor)
        {
            return FiltrarSimples(propriedade, valor);
        }

        /// <summary>
        /// Método de compatibilidade - use FiltrarSimples em vez disso
        /// </summary>
        [Obsolete("Use FiltrarSimples em vez disso")]
        public static QueryParamsBase BuscarPorPropriedade(string propriedade, Guid valor)
        {
            return FiltrarSimples(propriedade, valor);
        }

        #endregion
    }
} 