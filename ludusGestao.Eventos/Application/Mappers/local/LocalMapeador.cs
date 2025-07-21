using ludusGestao.Eventos.Domain.DTOs.Local;
using LudusGestao.Shared.Application.Mappers;
using LudusGestao.Shared.Domain.ValueObjects;
using ludusGestao.Eventos.Domain.Entities;

namespace ludusGestao.Eventos.Application.Mappers.local
{
    public class LocalMapeador : MapeadorBase<CriarLocalDTO, AtualizarLocalDTO, Local, LocalDTO>
    {
        public override Local Mapear(CriarLocalDTO dto)
        {
            // Mapeamento customizado
            return Local.Criar(dto.Nome, new Endereco(dto.Rua, dto.Numero, dto.Bairro, dto.Cidade, dto.Estado, dto.Cep), dto.Capacidade);
        }

        public override void Mapear(AtualizarLocalDTO dto, Local entidade)
        {
            entidade.Atualizar(dto.Nome, new Endereco(dto.Rua, dto.Numero, dto.Bairro, dto.Cidade, dto.Estado, dto.Cep), dto.Capacidade);
        }

        public override LocalDTO Mapear(Local entidade)
        {
            return new LocalDTO
            {
                Id = entidade.Id.ToString(),
                Nome = entidade.Nome,
                Rua = entidade.Endereco.Rua,
                Numero = entidade.Endereco.Numero,
                Bairro = entidade.Endereco.Bairro,
                Cidade = entidade.Endereco.Cidade,
                Estado = entidade.Endereco.Estado,
                Cep = entidade.Endereco.Cep,
                Capacidade = entidade.Capacidade,
                Ativo = entidade.Ativo
            };
        }
    }
} 