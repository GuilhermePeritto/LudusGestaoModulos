using System.Reflection;

namespace LudusGestao.Shared.Domain.QueryParams.Models
{
    /// <summary>
    /// Mapeamento de campos do front-end para propriedades de ValueObjects
    /// </summary>
    public static class MapeamentoValueObject
    {
        /// <summary>
        /// Mapeamentos padrão para ValueObjects comuns
        /// </summary>
        private static readonly Dictionary<string, Dictionary<string, string>> MapeamentosPadrao = new()
        {
            // Email
            ["Email"] = new Dictionary<string, string>
            {
                ["Email"] = "Email.Endereco"
            },
            
            // Cnpj
            ["Cnpj"] = new Dictionary<string, string>
            {
                ["Cnpj"] = "Cnpj.Numero"
            },
            
            // Cpf
            ["Cpf"] = new Dictionary<string, string>
            {
                ["Cpf"] = "Cpf.Numero"
            },
            
            // Telefone
            ["Telefone"] = new Dictionary<string, string>
            {
                ["Telefone"] = "Telefone.Numero"
            },
            
            // Endereco
            ["Endereco"] = new Dictionary<string, string>
            {
                ["Rua"] = "Endereco.Rua",
                ["Numero"] = "Endereco.Numero",
                ["Bairro"] = "Endereco.Bairro",
                ["Cidade"] = "Endereco.Cidade",
                ["Estado"] = "Endereco.Estado",
                ["Cep"] = "Endereco.Cep"
            }
        };

        /// <summary>
        /// Mapeia um campo do front-end para a propriedade real da entidade
        /// </summary>
        public static string MapearCampo<TEntity>(string campo) where TEntity : class
        {
            var tipoEntidade = typeof(TEntity);
            var nomeTipo = tipoEntidade.Name;

            // Verifica se existe mapeamento específico para o tipo
            if (MapeamentosPadrao.TryGetValue(nomeTipo, out var mapeamentos))
            {
                if (mapeamentos.TryGetValue(campo, out var propriedadeMapeada))
                {
                    return propriedadeMapeada;
                }
            }

            // Se não encontrar mapeamento, retorna o campo original
            return campo;
        }

        /// <summary>
        /// Mapeia uma lista de campos do front-end para propriedades reais da entidade
        /// </summary>
        public static List<string> MapearCampos<TEntity>(List<string> campos) where TEntity : class
        {
            return campos.Select(campo => MapearCampo<TEntity>(campo)).ToList();
        }

        /// <summary>
        /// Verifica se um campo é um ValueObject mapeado
        /// </summary>
        public static bool EhValueObjectMapeado<TEntity>(string campo) where TEntity : class
        {
            var campoMapeado = MapearCampo<TEntity>(campo);
            return campoMapeado != campo;
        }

        /// <summary>
        /// Obtém o valor de uma propriedade mapeada de ValueObject
        /// </summary>
        public static object? ObterValorValueObject<TEntity>(TEntity entidade, string campo) where TEntity : class
        {
            var campoMapeado = MapearCampo<TEntity>(campo);
            
            if (campoMapeado == campo)
                return null; // Não é um ValueObject mapeado

            var partes = campoMapeado.Split('.');
            if (partes.Length != 2)
                return null;

            var propriedadeValueObject = partes[0];
            var propriedadeInterna = partes[1];

            var tipoEntidade = typeof(TEntity);
            var propValueObject = tipoEntidade.GetProperty(propriedadeValueObject);
            
            if (propValueObject == null)
                return null;

            var valueObject = propValueObject.GetValue(entidade);
            if (valueObject == null)
                return null;

            var tipoValueObject = valueObject.GetType();
            var propInterna = tipoValueObject.GetProperty(propriedadeInterna);
            
            if (propInterna == null)
                return null;

            return propInterna.GetValue(valueObject);
        }

        /// <summary>
        /// Adiciona mapeamento customizado para um tipo
        /// </summary>
        public static void AdicionarMapeamento<TEntity>(Dictionary<string, string> mapeamento) where TEntity : class
        {
            var nomeTipo = typeof(TEntity).Name;
            MapeamentosPadrao[nomeTipo] = mapeamento;
        }

        /// <summary>
        /// Remove mapeamento de um tipo
        /// </summary>
        public static void RemoverMapeamento<TEntity>() where TEntity : class
        {
            var nomeTipo = typeof(TEntity).Name;
            MapeamentosPadrao.Remove(nomeTipo);
        }
    }
} 