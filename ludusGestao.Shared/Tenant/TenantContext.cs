using System.Threading;

namespace LudusGestao.Shared.Application.Providers
{
    public class TenantContext : ITenantContext
    {
        private static readonly AsyncLocal<int?> _tenantId = new AsyncLocal<int?>();
        public int TenantId => _tenantId.Value ?? throw new InvalidOperationException("TenantId não definido para o contexto atual.");
        public void SetTenantId(int tenantId) => _tenantId.Value = tenantId;

        private static readonly AsyncLocal<bool> _ignorarFiltro = new AsyncLocal<bool>();
        public bool IgnorarFiltroTenant => _ignorarFiltro.Value;
        public void IgnorarFiltro(bool ignorar) => _ignorarFiltro.Value = ignorar;
    }
} 