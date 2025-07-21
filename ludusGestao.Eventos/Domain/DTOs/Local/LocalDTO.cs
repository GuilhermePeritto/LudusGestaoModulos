namespace ludusGestao.Eventos.Domain.DTOs.Local
{
    public class LocalDTO
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Cep { get; set; }
        public int Capacidade { get; set; }
        public bool Ativo { get; set; }
    }
} 