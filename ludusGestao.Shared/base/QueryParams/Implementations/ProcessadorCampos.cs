using System.Reflection;
using LudusGestao.Shared.Domain.QueryParams.Interfaces;
using LudusGestao.Shared.Domain.QueryParams.Exceptions;
using LudusGestao.Shared.Domain.QueryParams.Models;

namespace LudusGestao.Shared.Domain.QueryParams.Implementations
{
    /// <summary>
    /// Implementação do processador de campos
    /// </summary>
    public class ProcessadorCampos : IProcessadorCampos
    {
        /// <summary>
        /// Processa campos específicos e retorna lista de propriedades válidas
        /// </summary>
        public List<string> ProcessarCampos<TEntity>(string? campos) where TEntity : class
        {
            if (string.IsNullOrWhiteSpace(campos))
                return new List<string>();

            var camposProcessados = campos
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.Trim())
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .ToList();

            // Mapeia campos do front-end para propriedades de ValueObjects
            return MapeamentoValueObject.MapearCampos<TEntity>(camposProcessados);
        }

        /// <summary>
        /// Aplica filtro de campos em uma entidade
        /// </summary>
        public object AplicarFiltroCampos<TEntity>(TEntity entidade, List<string> campos) where TEntity : class
        {
            if (entidade == null)
                return new { };

            if (!campos.Any())
                return entidade;

            var resultado = new Dictionary<string, object?>();
            var tipoEntidade = typeof(TEntity);

            foreach (var campo in campos)
            {
                var valor = ObterValorCampo(entidade, campo, tipoEntidade);
                resultado[campo] = valor;
            }

            return resultado;
        }

        /// <summary>
        /// Aplica filtro de campos em uma coleção de entidades
        /// </summary>
        public IEnumerable<object> AplicarFiltroCampos<TEntity>(IEnumerable<TEntity> entidades, List<string> campos) where TEntity : class
        {
            if (!campos.Any())
                return entidades.Cast<object>();

            return entidades.Select(entidade => AplicarFiltroCampos(entidade, campos));
        }

        /// <summary>
        /// Aplica filtro de campos de forma performática retornando EntidadeDinamicaDTO
        /// </summary>
        public IQueryable<EntidadeDinamicaDTO> AplicarFiltroCamposComoDTO<TEntity>(IQueryable<TEntity> query, List<string> campos) where TEntity : class
        {
            if (!campos.Any())
                return query.Select(e => EntidadeDinamicaDTO.FromEntity(e, new List<string>()));

            // Abordagem simplificada: executa a query completa e depois filtra os campos
            return query.Select(e => EntidadeDinamicaDTO.FromEntity(e, campos));
        }

        /// <summary>
        /// Valida se os campos existem na entidade
        /// </summary>
        public List<string> ValidarCampos<TEntity>(List<string> campos) where TEntity : class
        {
            if (campos == null || !campos.Any())
                return new List<string>();

            var tipoEntidade = typeof(TEntity);
            var camposValidos = new List<string>();

            foreach (var campo in campos)
            {
                if (CampoExiste<TEntity>(campo))
                {
                    camposValidos.Add(campo);
                }
            }

            return camposValidos;
        }

        /// <summary>
        /// Obtém todas as propriedades públicas de uma entidade
        /// </summary>
        public List<string> ObterTodasPropriedades<TEntity>() where TEntity : class
        {
            var tipoEntidade = typeof(TEntity);
            return tipoEntidade.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p => p.Name)
                .ToList();
        }

        /// <summary>
        /// Verifica se um campo existe na entidade
        /// </summary>
        public bool CampoExiste<TEntity>(string campo) where TEntity : class
        {
            if (string.IsNullOrWhiteSpace(campo))
                return false;

            var tipoEntidade = typeof(TEntity);
            
            // Verifica se é um campo simples
            var propriedade = tipoEntidade.GetProperty(campo, 
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            
            if (propriedade != null)
                return true;

            // Verifica se é um campo de ValueObject (formato: Propriedade.SubPropriedade)
            var partes = campo.Split('.');
            if (partes.Length == 2)
            {
                var propValueObject = tipoEntidade.GetProperty(partes[0], 
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                
                if (propValueObject != null)
                {
                    var tipoValueObject = propValueObject.PropertyType;
                    var propInterna = tipoValueObject.GetProperty(partes[1], 
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    
                    return propInterna != null;
                }
            }

            return false;
        }

        /// <summary>
        /// Obtém o tipo de uma propriedade
        /// </summary>
        public Type? ObterTipoPropriedade<TEntity>(string propriedade) where TEntity : class
        {
            if (string.IsNullOrWhiteSpace(propriedade))
                return null;

            var tipoEntidade = typeof(TEntity);
            
            // Verifica se é um campo simples
            var prop = tipoEntidade.GetProperty(propriedade, 
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            
            if (prop != null)
                return prop.PropertyType;

            // Verifica se é um campo de ValueObject
            var partes = propriedade.Split('.');
            if (partes.Length == 2)
            {
                var propValueObject = tipoEntidade.GetProperty(partes[0], 
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                
                if (propValueObject != null)
                {
                    var tipoValueObject = propValueObject.PropertyType;
                    var propInterna = tipoValueObject.GetProperty(partes[1], 
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    
                    return propInterna?.PropertyType;
                }
            }

            return null;
        }

        /// <summary>
        /// Obtém o valor de um campo (suporta ValueObjects)
        /// </summary>
        private object? ObterValorCampo<TEntity>(TEntity entidade, string campo, Type tipoEntidade) where TEntity : class
        {
            // Verifica se é um campo simples
            var propriedade = tipoEntidade.GetProperty(campo, 
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            
            if (propriedade != null)
            {
                return propriedade.GetValue(entidade);
            }

            // Verifica se é um campo de ValueObject
            var partes = campo.Split('.');
            if (partes.Length == 2)
            {
                var propValueObject = tipoEntidade.GetProperty(partes[0], 
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                
                if (propValueObject != null)
                {
                    var valueObject = propValueObject.GetValue(entidade);
                    if (valueObject != null)
                    {
                        var tipoValueObject = valueObject.GetType();
                        var propInterna = tipoValueObject.GetProperty(partes[1], 
                            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        
                        if (propInterna != null)
                        {
                            return propInterna.GetValue(valueObject);
                        }
                    }
                }
            }

            return null;
        }
    }
} 