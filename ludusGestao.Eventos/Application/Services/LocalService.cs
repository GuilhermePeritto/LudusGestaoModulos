using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Notificacao;
using ludusGestao.Eventos.Domain.Entities.Local.Interfaces;
using ludusGestao.Eventos.Domain.Entities.Local.DTOs;
using ludusGestao.Eventos.Domain.Entities.Local;

namespace ludusGestao.Eventos.Application.Services
{
    public class LocalService : BaseService, ILocalService
    {
        private readonly ICriarLocalUseCase _criarLocalUseCase;
        private readonly IListarLocaisUseCase _listarLocaisUseCase;
        private readonly IBuscarLocalPorIdUseCase _buscarLocalPorIdUseCase;
        private readonly IAtualizarLocalUseCase _atualizarLocalUseCase;
        private readonly IRemoverLocalUseCase _removerLocalUseCase;

        public LocalService(
            ICriarLocalUseCase criarLocalUseCase,
            IListarLocaisUseCase listarLocaisUseCase,
            IBuscarLocalPorIdUseCase buscarLocalPorIdUseCase,
            IAtualizarLocalUseCase atualizarLocalUseCase,
            IRemoverLocalUseCase removerLocalUseCase,
            INotificador notificador)
            : base(notificador)
        {
            _criarLocalUseCase = criarLocalUseCase;
            _listarLocaisUseCase = listarLocaisUseCase;
            _buscarLocalPorIdUseCase = buscarLocalPorIdUseCase;
            _atualizarLocalUseCase = atualizarLocalUseCase;
            _removerLocalUseCase = removerLocalUseCase;
        }

        public async Task<LocalDTO> Criar(CriarLocalDTO dto)
        {
            var local = await _criarLocalUseCase.Executar(dto);
            
            if (local == null)
                return null;

            return LocalDTO.Criar(local);
        }

        public async Task<IEnumerable<LocalDTO>> Listar()
        {
            var locais = await _listarLocaisUseCase.Executar();
            return locais.Select(LocalDTO.Criar);
        }

        public async Task<LocalDTO> BuscarPorId(Guid id)
        {
            var local = await _buscarLocalPorIdUseCase.Executar(id);
            
            // Se o BuscarLocalPorIdUseCase retornou null, ele já notificou o erro
            if (local == null)
                return null;

            return LocalDTO.Criar(local);
        }

        public async Task<LocalDTO> Atualizar(Guid id, AtualizarLocalDTO dto)
        {
            // Primeiro, buscar o local existente
            var localExistente = await _buscarLocalPorIdUseCase.Executar(id);
            
            // Se o BuscarLocalPorIdUseCase retornou null, ele já notificou o erro
            if (localExistente == null)
                return null;

            // Atualizar os dados do local existente
            var endereco = new LudusGestao.Shared.Domain.ValueObjects.Endereco(dto.Rua, dto.Numero, dto.Bairro, dto.Cidade, dto.Estado, dto.Cep);
            var telefone = new LudusGestao.Shared.Domain.ValueObjects.Telefone(dto.Telefone);
            
            localExistente.Atualizar(dto.Nome, dto.Descricao, endereco, telefone);
            
            // Executar o use case de atualização
            var localAtualizado = await _atualizarLocalUseCase.Executar(localExistente);
            
            if (localAtualizado == null)
                return null;

            return LocalDTO.Criar(localAtualizado);
        }

        public async Task<bool> Remover(Guid id)
        {
            return await _removerLocalUseCase.Executar(id);
        }
    }
} 