using System.Threading.Tasks;

namespace LudusGestao.Shared.Tenant
{
    public interface ITenantResolver
    {
        Task<TenantInfo> ResolveTenantAsync(string tenantId);
        Task<bool> IsValidTenantAsync(int tenantId);
        Task<bool> IsPublicRouteAsync(string path);
    }

    public class TenantInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string ConnectionString { get; set; }
        public Dictionary<string, object> Settings { get; set; } = new();
    }
} 