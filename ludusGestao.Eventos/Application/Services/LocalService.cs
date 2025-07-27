using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.Entities.Local.Interfaces;
using ludusGestao.Eventos.Domain.Entities.Local.DTOs;
using LudusGestao.Shared.Notificacao;

namespace ludusGestao.Eventos.Application.Services
{
    public class LocalService : ILocalService
    {
        private readonly ICriarLocalUseCase _criarLocalUseCase;
        private readonly IListarLocaisUseCase _listarLocaisUseCase;
        private readonly IBuscarLocalPorIdUseCase _buscarLocalPorIdUseCase;
        private readonly IAtualizarLocalUseCase _atualizarLocalUseCase;
        private readonly IRemoverLocalUseCase _removerLocalUseCase;
        private readonly INotificador _notificador;

        public LocalService(
            ICriarLocalUseCase criarLocalUseCase,
            IListarLocaisUseCase listarLocaisUseCase,
            IBuscarLocalPorIdUseCase buscarLocalPorIdUseCase,
            IAtualizarLocalUseCase atualizarLocalUseCase,
            IRemoverLocalUseCase removerLocalUseCase,
            INotificador notificador)
        {
            _criarLocalUseCase = criarLocalUseCase;
            _listarLocaisUseCase = listarLocaisUseCase;
            _buscarLocalPorIdUseCase = buscarLocalPorIdUseCase;
            _atualizarLocalUseCase = atualizarLocalUseCase;
            _removerLocalUseCase = removerLocalUseCase;
            _notificador = notificador;
        }

        public async Task<ludusGestao.Eventos.Domain.Entities.Local.Local> Criar(CriarLocalDTO dto)
        {
            return await _criarLocalUseCase.Executar(dto);
        }

        public async Task<IEnumerable<ludusGestao.Eventos.Domain.Entities.Local.Local>> Listar()
        {
            return await _listarLocaisUseCase.Executar();
        }

        public async Task<ludusGestao.Eventos.Domain.Entities.Local.Local> BuscarPorId(Guid id)
        {
            return await _buscarLocalPorIdUseCase.Executar(id);
        }

        public async Task<ludusGestao.Eventos.Domain.Entities.Local.Local> Atualizar(Guid id, AtualizarLocalDTO dto)
        {
            // Primeiro, buscar o local existente
            var localExistente = await _buscarLocalPorIdUseCase.Executar(id);
            if (localExistente == null)
                return null;

            // Atualizar os dados do local existente
            var endereco = new LudusGestao.Shared.Domain.ValueObjects.Endereco(dto.Rua, dto.Numero, dto.Bairro, dto.Cidade, dto.Estado, dto.Cep);
            var telefone = new LudusGestao.Shared.Domain.ValueObjects.Telefone(dto.Telefone);
            
            localExistente.Atualizar(dto.Nome, dto.Descricao, endereco, telefone);
            
            // Executar o use case de atualização
            return await _atualizarLocalUseCase.Executar(localExistente);
        }

        public async Task Remover(Guid id)
        {
            await _removerLocalUseCase.Executar(id);
        }
    }
} 