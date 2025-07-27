namespace LudusGestao.Shared.Tenant
{
    public interface ITenantContext
    {
        int TenantId { get; }
        int? TenantIdNullable { get; }
        TenantInfo TenantInfo { get; }
        void SetTenantId(int tenantId);
        void SetTenantInfo(TenantInfo tenantInfo);
        bool IgnorarFiltroTenant { get; }
        void IgnorarFiltro(bool ignorar);
        void Clear();
    }
} 