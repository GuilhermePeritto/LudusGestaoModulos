using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.Entities.Local.Interfaces;
using ludusGestao.Eventos.Domain.Entities.Local.DTOs;
using LudusGestao.Shared.Domain.ValueObjects;
using LudusGestao.Shared.Notificacao;

namespace ludusGestao.Eventos.Domain.Entities.Local.UseCases
{
    public class CriarLocalUseCase : ICriarLocalUseCase
    {
        private readonly ILocalWriteProvider _localWriteProvider;
        private readonly INotificador _notificador;

        public CriarLocalUseCase(ILocalWriteProvider localWriteProvider, INotificador notificador)
        {
            _localWriteProvider = localWriteProvider;
            _notificador = notificador;
        }

        public async Task<Local> Executar(CriarLocalDTO dto)
        {
            var endereco = new Endereco(dto.Rua, dto.Numero, dto.Bairro, dto.Cidade, dto.Estado, dto.Cep);
            var telefone = new Telefone(dto.Telefone);

            var local = new Local(dto.Nome, dto.Descricao, endereco, telefone);

            await _localWriteProvider.Adicionar(local);
            await _localWriteProvider.SalvarAlteracoes();

            return local;
        }
    }
} 