using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Notificacao;
using ludusGestao.Eventos.Domain.Entities.Local;
using ludusGestao.Eventos.Domain.Entities.Local.Interfaces;
using ludusGestao.Eventos.Domain.Entities.Local.DTOs;
using ludusGestao.Eventos.Domain.Entities.Local.Validations;
using LudusGestao.Shared.Domain.ValueObjects;

namespace ludusGestao.Eventos.Domain.Entities.Local.UseCases
{
    public class CriarLocalUseCase : BaseUseCase, ICriarLocalUseCase
    {
        private readonly ILocalWriteProvider _localWriteProvider;

        public CriarLocalUseCase(ILocalWriteProvider localWriteProvider, INotificador notificador)
            : base(notificador)
        {
            _localWriteProvider = localWriteProvider;
        }

        public async Task<Local> Executar(CriarLocalDTO dto)
        {
            var endereco = new Endereco(dto.Rua, dto.Numero, dto.Bairro, dto.Cidade, dto.Estado, dto.Cep);
            var telefone = new Telefone(dto.Telefone);

            var local = Local.Criar(dto.Nome, dto.Descricao, endereco, telefone);

            if (!ExecutarValidacao(new CriarLocalValidation(), local))
                return null;

            await _localWriteProvider.Adicionar(local);
            await _localWriteProvider.SalvarAlteracoes();

            return local;
        }
    }
} 