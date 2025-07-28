using System.Dynamic;
using System.Reflection;

namespace LudusGestao.Shared.Domain.Common
{
    /// <summary>
    /// DTO dinâmico que pode ser construído com campos específicos de uma entidade
    /// </summary>
    public class DynamicEntityDTO : DynamicObject
    {
        private readonly Dictionary<string, object> _properties;

        public DynamicEntityDTO()
        {
            _properties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Constrói um DTO dinâmico a partir de uma entidade e lista de campos
        /// </summary>
        public static DynamicEntityDTO FromEntity<TEntity>(TEntity entity, List<string> fields) where TEntity : class
        {
            if (entity == null)
                return new DynamicEntityDTO();

            var dto = new DynamicEntityDTO();
            var entityType = typeof(TEntity);

            foreach (var field in fields)
            {
                var prop = entityType.GetProperty(field, 
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                
                if (prop != null)
                {
                    var value = prop.GetValue(entity);
                    dto._properties[field] = value;
                }
            }

            return dto;
        }

        /// <summary>
        /// Constrói uma lista de DTOs dinâmicos a partir de uma lista de entidades
        /// </summary>
        public static List<DynamicEntityDTO> FromEntities<TEntity>(IEnumerable<TEntity> entities, List<string> fields) where TEntity : class
        {
            return entities.Select(entity => FromEntity(entity, fields)).ToList();
        }

        /// <summary>
        /// Adiciona uma propriedade ao DTO
        /// </summary>
        public void AddProperty(string name, object value)
        {
            _properties[name] = value;
        }

        /// <summary>
        /// Obtém uma propriedade do DTO
        /// </summary>
        public object GetProperty(string name)
        {
            return _properties.TryGetValue(name, out var value) ? value : null;
        }

        /// <summary>
        /// Verifica se o DTO tem uma propriedade
        /// </summary>
        public bool HasProperty(string name)
        {
            return _properties.ContainsKey(name);
        }

        /// <summary>
        /// Obtém todas as propriedades como Dictionary
        /// </summary>
        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>(_properties);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = GetProperty(binder.Name);
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            AddProperty(binder.Name, value);
            return true;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _properties.Keys;
        }

        /// <summary>
        /// Converte para objeto anônimo
        /// </summary>
        public object ToAnonymous()
        {
            return _properties;
        }
    }
} 