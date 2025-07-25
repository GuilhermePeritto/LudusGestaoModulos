using LudusGestao.Shared.Domain.Common;

namespace LudusGestao.Shared.Domain.Providers
{
    public static class QueryParamsHelper
    {
        public static QueryParamsBase BuscarPorId(Guid id)
        {
            return new QueryParamsBase { Filter = $"Id:{id}" };
        }

        public static QueryParamsBase BuscarPorId(string id)
        {
            return new QueryParamsBase { Filter = $"Id:{id}" };
        }

        public static QueryParamsBase BuscarPorPropriedade(string propriedade, string valor)
        {
            return new QueryParamsBase { Filter = $"{propriedade}:{valor}" };
        }

        public static QueryParamsBase BuscarPorPropriedade(string propriedade, int valor)
        {
            return new QueryParamsBase { Filter = $"{propriedade}:{valor}" };
        }

        public static QueryParamsBase BuscarPorPropriedade(string propriedade, Guid valor)
        {
            return new QueryParamsBase { Filter = $"{propriedade}:{valor}" };
        }

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

        public static QueryParamsBase FiltrarPorId(Guid id)
        {
            return Filtrar("Id", id, "eq");
        }

        public static QueryParamsBase FiltrarPorId(string id)
        {
            return Filtrar("Id", id, "eq");
        }

        public static QueryParamsBase FiltrarPorNome(string nome, string operador = "like")
        {
            return Filtrar("Nome", nome, operador);
        }

        public static QueryParamsBase FiltrarPorAtivo(bool ativo)
        {
            return Filtrar("Ativo", ativo, "eq");
        }

        public static QueryParamsBase FiltrarPorTenant(int tenantId)
        {
            return Filtrar("TenantId", tenantId, "eq");
        }

        public static QueryParamsBase OrdenarPor(string propriedade)
        {
            return new QueryParamsBase { Sort = propriedade };
        }

        public static QueryParamsBase OrdenarPorNome()
        {
            return OrdenarPor("Nome");
        }

        public static QueryParamsBase OrdenarPorId()
        {
            return OrdenarPor("Id");
        }

        public static QueryParamsBase Paginar(int pagina = 1, int limite = 10)
        {
            return new QueryParamsBase 
            { 
                Page = pagina,
                Limit = limite
            };
        }

        public static QueryParamsBase ListarComPaginacao(int pagina = 1, int limite = 10)
        {
            return new QueryParamsBase 
            { 
                Page = pagina,
                Limit = limite
            };
        }

        public static QueryParamsBase ListarComOrdenacao(string propriedadeOrdenacao, int pagina = 1, int limite = 10)
        {
            return new QueryParamsBase 
            { 
                Sort = propriedadeOrdenacao,
                Page = pagina,
                Limit = limite
            };
        }

        public static QueryParamsBase ListarComFiltroEPaginacao(string propriedade, string valor, int pagina = 1, int limite = 10)
        {
            return new QueryParamsBase 
            { 
                Filter = $"{propriedade}:{valor}",
                Page = pagina,
                Limit = limite
            };
        }

        public static QueryParamsBase ListarComFiltroEPaginacao(string propriedade, int valor, int pagina = 1, int limite = 10)
        {
            return new QueryParamsBase 
            { 
                Filter = $"{propriedade}:{valor}",
                Page = pagina,
                Limit = limite
            };
        }

        public static QueryParamsBase ListarComFiltroEPaginacao(string propriedade, Guid valor, int pagina = 1, int limite = 10)
        {
            return new QueryParamsBase 
            { 
                Filter = $"{propriedade}:{valor}",
                Page = pagina,
                Limit = limite
            };
        }

        public static QueryParamsBase ListarCompleto(string propriedade, string valor, string ordenacao, int pagina = 1, int limite = 10)
        {
            return new QueryParamsBase 
            { 
                Filter = $"{propriedade}:{valor}",
                Sort = ordenacao,
                Page = pagina,
                Limit = limite
            };
        }

        public static QueryParamsBase ListarCompleto(string propriedade, int valor, string ordenacao, int pagina = 1, int limite = 10)
        {
            return new QueryParamsBase 
            { 
                Filter = $"{propriedade}:{valor}",
                Sort = ordenacao,
                Page = pagina,
                Limit = limite
            };
        }

        public static QueryParamsBase ListarCompleto(string propriedade, Guid valor, string ordenacao, int pagina = 1, int limite = 10)
        {
            return new QueryParamsBase 
            { 
                Filter = $"{propriedade}:{valor}",
                Sort = ordenacao,
                Page = pagina,
                Limit = limite
            };
        }

        public static QueryParamsBase ListarTodos()
        {
            return new QueryParamsBase();
        }

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
                    // Se já tem um filtro simples, converte para FilterObjects
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

        public static QueryParamsBase FiltrarEOrdenar(string propriedade, object valor, string ordenacao, string operador = "eq")
        {
            return Combinar(
                Filtrar(propriedade, valor, operador),
                OrdenarPor(ordenacao)
            );
        }

        public static QueryParamsBase FiltrarOrdenarEPaginar(string propriedade, object valor, string ordenacao, int pagina = 1, int limite = 10, string operador = "eq")
        {
            return Combinar(
                Filtrar(propriedade, valor, operador),
                OrdenarPor(ordenacao),
                Paginar(pagina, limite)
            );
        }
    }
} 