using System.Threading;

namespace LudusGestao.Shared.Tenant
{
    public class TenantContext : ITenantContext
    {
        private static readonly AsyncLocal<int?> _tenantId = new AsyncLocal<int?>();
        private static readonly AsyncLocal<TenantInfo> _tenantInfo = new AsyncLocal<TenantInfo>();
        private static readonly AsyncLocal<bool> _ignorarFiltro = new AsyncLocal<bool>();
        
        public int TenantId => _tenantId.Value ?? throw new InvalidOperationException("TenantId não definido para o contexto atual.");
        public TenantInfo TenantInfo => _tenantInfo.Value;
        
        public void SetTenantId(int tenantId) => _tenantId.Value = tenantId;
        public void SetTenantInfo(TenantInfo tenantInfo) => _tenantInfo.Value = tenantInfo;
        
        public bool IgnorarFiltroTenant => _ignorarFiltro.Value;
        public void IgnorarFiltro(bool ignorar) => _ignorarFiltro.Value = ignorar;
        
        public void Clear()
        {
            _tenantId.Value = null;
            _tenantInfo.Value = null;
            _ignorarFiltro.Value = false;
        }
    }
} 