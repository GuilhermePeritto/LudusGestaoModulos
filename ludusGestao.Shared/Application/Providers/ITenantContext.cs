namespace LudusGestao.Shared.Application.Providers
{
    public interface ITenantContext
    {
        int TenantId { get; }
        void SetTenantId(int tenantId);
    }
} 