using ludusGestao.Gerais.Domain.Empresa.DTOs;
using ludusGestao.Gerais.Domain.Filial.DTOs;
using ludusGestao.Gerais.Domain.Usuario.DTOs;

namespace ludusGestao.Gestao.Domain.Entities.Cliente.DTOs
{
    public class ClienteCriadoDTO
    {
        public int TenantId { get; set; }
        public EmpresaDTO Empresa { get; set; } = null!;
        public FilialDTO Filial { get; set; } = null!;
        public UsuarioDTO Usuario { get; set; } = null!;
        public string SenhaAdministrador { get; set; } = string.Empty;
        public string EmailAdministrador { get; set; } = string.Empty;
    }
} 