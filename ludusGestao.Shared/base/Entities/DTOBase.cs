using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LudusGestao.Shared.Domain.Entities
{
    /// <summary>
    /// DTO base que fornece sobrecarga do método Criar para campos específicos
    /// </summary>
    public abstract class DTOBase
    {
        public Guid Id { get; protected set; }
        public DateTime DataCriacao { get; protected set; }
        public DateTime? DataAlteracao { get; protected set; }
        public int TenantId { get; protected set; }

        protected DTOBase()
        {
        }

        /// <summary>
        /// Cria um DTO a partir de um objeto genérico, permitindo especificar campos específicos
        /// Esta é uma sobrecarga do método Criar padrão dos DTOs
        /// </summary>
        /// <typeparam name="TDto">Tipo do DTO</typeparam>
        /// <param name="entity">Objeto fonte (entidade)</param>
        /// <param name="fields">Lista de campos a serem incluídos (opcional)</param>
        /// <returns>DTO com os campos especificados</returns>
        public static TDto Criar<TDto>(object entity, List<string> fields = null) 
            where TDto : DTOBase, new()
        {
            if (entity == null)
                return null;

            var dto = new TDto();
            var entityType = entity.GetType();
            var dtoType = typeof(TDto);

            // Se não foram especificados campos, incluir todos os campos públicos
            if (fields == null || !fields.Any())
            {
                fields = dtoType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanWrite)
                    .Select(p => p.Name)
                    .ToList();
            }

            foreach (var field in fields)
            {
                var dtoProp = dtoType.GetProperty(field, 
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                
                if (dtoProp != null && dtoProp.CanWrite)
                {
                    var entityProp = entityType.GetProperty(field, 
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    
                    if (entityProp != null)
                    {
                        var value = entityProp.GetValue(entity);
                        dtoProp.SetValue(dto, value);
                    }
                }
            }

            return dto;
        }

        /// <summary>
        /// Cria um DTO com campos específicos baseado em uma string separada por vírgulas
        /// </summary>
        /// <typeparam name="TDto">Tipo do DTO</typeparam>
        /// <param name="entity">Objeto fonte (entidade)</param>
        /// <param name="fieldsString">String com campos separados por vírgula</param>
        /// <returns>DTO com os campos especificados</returns>
        public static TDto Criar<TDto>(object entity, string fieldsString) 
            where TDto : DTOBase, new()
        {
            if (string.IsNullOrWhiteSpace(fieldsString))
                return Criar<TDto>(entity);

            var fields = fieldsString.Split(',')
                .Select(f => f.Trim())
                .Where(f => !string.IsNullOrEmpty(f))
                .ToList();

            return Criar<TDto>(entity, fields);
        }


    }
} 