using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;
using LudusGestao.Shared.Domain.Entities;

namespace LudusGestao.Shared.Tenant
{
    public static class TenantFilterBuilder
    {
        private static readonly Dictionary<Type, LambdaExpression> _filterCache = new();
        private static readonly object _cacheLock = new();

        public static void ApplyTenantFilter<TEntity>(EntityTypeBuilder<TEntity> builder, int? tenantId, bool ignorarFiltro = false) 
            where TEntity : EntidadeBase
        {
            if (ignorarFiltro || !tenantId.HasValue) 
            {
                // Remover qualquer filtro existente quando ignorarFiltro é true ou tenantId é null
                builder.HasQueryFilter(null);
                return;
            }

            var filter = GetOrCreateFilter<TEntity>(tenantId.Value);
            builder.HasQueryFilter(filter);
        }

        private static Expression<Func<TEntity, bool>> GetOrCreateFilter<TEntity>(int tenantId) where TEntity : EntidadeBase
        {
            var entityType = typeof(TEntity);
            
            lock (_cacheLock)
            {
                if (_filterCache.TryGetValue(entityType, out var cachedFilter))
                {
                    return (Expression<Func<TEntity, bool>>)cachedFilter;
                }

                var parameter = Expression.Parameter(entityType, "e");
                var tenantIdProperty = Expression.Property(parameter, nameof(EntidadeBase.TenantId));
                var tenantIdConstant = Expression.Constant(tenantId);
                var equalExpression = Expression.Equal(tenantIdProperty, tenantIdConstant);
                
                var filter = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);
                
                _filterCache[entityType] = filter;
                return filter;
            }
        }

        public static void ClearCache()
        {
            lock (_cacheLock)
            {
                _filterCache.Clear();
            }
        }
    }
} 