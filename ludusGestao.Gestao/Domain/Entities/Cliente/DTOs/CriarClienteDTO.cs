namespace ludusGestao.Gestao.Domain.Entities.Cliente.DTOs
{
    public class CriarClienteDTO
    {
        public string NomeEmpresa { get; set; } = string.Empty;
        public string EmailEmpresa { get; set; } = string.Empty;
        public string TelefoneEmpresa { get; set; } = string.Empty;
        public string CnpjEmpresa { get; set; } = string.Empty;
        public string RuaEmpresa { get; set; } = string.Empty;
        public string NumeroEmpresa { get; set; } = string.Empty;
        public string BairroEmpresa { get; set; } = string.Empty;
        public string CidadeEmpresa { get; set; } = string.Empty;
        public string EstadoEmpresa { get; set; } = string.Empty;
        public string CepEmpresa { get; set; } = string.Empty;
        
        public string ResponsavelFilial { get; set; } = string.Empty;
        public DateTime DataAberturaFilial { get; set; } = DateTime.Today;
    }
} 