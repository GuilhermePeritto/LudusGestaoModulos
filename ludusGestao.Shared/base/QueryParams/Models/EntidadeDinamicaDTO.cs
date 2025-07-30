using System.Dynamic;
using System.Reflection;
using LudusGestao.Shared.Domain.Common;

namespace LudusGestao.Shared.Domain.QueryParams.Models
{
    /// <summary>
    /// DTO dinâmico para retornar apenas os campos solicitados de uma entidade
    /// </summary>
    public class EntidadeDinamicaDTO : DynamicObject
    {
        private readonly Dictionary<string, object?> _propriedades;

        public EntidadeDinamicaDTO()
        {
            _propriedades = new Dictionary<string, object?>();
        }

        public EntidadeDinamicaDTO(Dictionary<string, object?> propriedades)
        {
            _propriedades = propriedades ?? new Dictionary<string, object?>();
        }

        /// <summary>
        /// Cria uma EntidadeDinamicaDTO a partir de uma entidade e lista de campos
        /// </summary>
        public static EntidadeDinamicaDTO FromEntity<TEntity>(TEntity entity, List<string> campos) where TEntity : class
        {
            if (entity == null)
                return new EntidadeDinamicaDTO();

            var tipo = typeof(TEntity);
            var propriedades = new Dictionary<string, object?>();

            if (campos == null || !campos.Any())
            {
                // Se não especificar campos, retorna todas as propriedades públicas
                var todasPropriedades = tipo.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var prop in todasPropriedades)
                {
                    try
                    {
                        propriedades[prop.Name] = prop.GetValue(entity);
                    }
                    catch
                    {
                        // Se não conseguir acessar a propriedade, define como null
                        propriedades[prop.Name] = null;
                    }
                }
            }
            else
            {
                // Retorna apenas os campos especificados
                foreach (var campo in campos)
                {
                    var propriedade = tipo.GetProperty(campo, 
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    
                    if (propriedade != null)
                    {
                        try
                        {
                            propriedades[propriedade.Name] = propriedade.GetValue(entity);
                        }
                        catch
                        {
                            // Se não conseguir acessar a propriedade, define como null
                            propriedades[propriedade.Name] = null;
                        }
                    }
                }
            }

            return new EntidadeDinamicaDTO(propriedades);
        }

        /// <summary>
        /// Cria uma EntidadeDinamicaDTO a partir de um objeto anônimo
        /// </summary>
        public static EntidadeDinamicaDTO FromAnonymousObject(object obj, List<string> campos)
        {
            if (obj == null)
                return new EntidadeDinamicaDTO();

            var tipo = obj.GetType();
            var propriedades = new Dictionary<string, object?>();

            if (campos == null || !campos.Any())
            {
                // Se não especificar campos, retorna todas as propriedades
                var todasPropriedades = tipo.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var prop in todasPropriedades)
                {
                    try
                    {
                        propriedades[prop.Name] = prop.GetValue(obj);
                    }
                    catch
                    {
                        propriedades[prop.Name] = null;
                    }
                }
            }
            else
            {
                // Retorna apenas os campos especificados
                foreach (var campo in campos)
                {
                    var propriedade = tipo.GetProperty(campo, 
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    
                    if (propriedade != null)
                    {
                        try
                        {
                            propriedades[propriedade.Name] = propriedade.GetValue(obj);
                        }
                        catch
                        {
                            propriedades[propriedade.Name] = null;
                        }
                    }
                }
            }

            return new EntidadeDinamicaDTO(propriedades);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object? result)
        {
            return _propriedades.TryGetValue(binder.Name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object? value)
        {
            _propriedades[binder.Name] = value;
            return true;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _propriedades.Keys;
        }

        /// <summary>
        /// Obtém o valor de uma propriedade
        /// </summary>
        public object? GetValue(string propriedade)
        {
            return _propriedades.TryGetValue(propriedade, out var valor) ? valor : null;
        }

        /// <summary>
        /// Define o valor de uma propriedade
        /// </summary>
        public void SetValue(string propriedade, object? valor)
        {
            _propriedades[propriedade] = valor;
        }

        /// <summary>
        /// Obtém todas as propriedades
        /// </summary>
        public Dictionary<string, object?> GetPropriedades()
        {
            return new Dictionary<string, object?>(_propriedades);
        }
    }
} 