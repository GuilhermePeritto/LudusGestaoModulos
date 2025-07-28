using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.QueryParams;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Notificacao;
using ludusGestao.Gerais.Domain.Filial.DTOs;
using ludusGestao.Gerais.Domain.Filial.Interfaces;
using ludusGestao.Gerais.Domain.Filial;

namespace ludusGestao.Gerais.Application.Services
{
    public class FilialService : BaseService, IFilialService
    {
        private readonly ICriarFilialUseCase _criarUseCase;
        private readonly IAtualizarFilialUseCase _atualizarUseCase;
        private readonly IRemoverFilialUseCase _removerUseCase;
        private readonly IBuscarFilialPorIdUseCase _buscarPorIdUseCase;
        private readonly IListarFiliaisUseCase _listarUseCase;

        public FilialService(
            ICriarFilialUseCase criarUseCase,
            IAtualizarFilialUseCase atualizarUseCase,
            IRemoverFilialUseCase removerUseCase,
            IBuscarFilialPorIdUseCase buscarPorIdUseCase,
            IListarFiliaisUseCase listarUseCase,
            INotificador notificador)
            : base(notificador)
        {
            _criarUseCase = criarUseCase;
            _atualizarUseCase = atualizarUseCase;
            _removerUseCase = removerUseCase;
            _buscarPorIdUseCase = buscarPorIdUseCase;
            _listarUseCase = listarUseCase;
        }

        public async Task<FilialDTO> Criar(CriarFilialDTO dto)
        {
            var filial = Filial.Criar(dto.Nome, dto.Rua, dto.Numero, dto.Bairro, dto.Cidade, dto.Estado, dto.Cep, dto.Telefone, dto.Email, dto.Cnpj, dto.Responsavel, dto.DataAbertura, dto.EmpresaId);
            var filialCriada = await _criarUseCase.Executar(filial);
            
            if (filialCriada == null)
                return null;

            return FilialDTO.Criar(filialCriada);
        }

        public async Task<FilialDTO> Atualizar(Guid id, AtualizarFilialDTO dto)
        {
            var filial = await _buscarPorIdUseCase.Executar(id);
            
            // Se o BuscarPorIdUseCase retornou null, ele já notificou o erro
            if (filial == null)
                return null;

            filial.Atualizar(dto.Nome, dto.Rua, dto.Numero, dto.Bairro, dto.Cidade, dto.Estado, dto.Cep, dto.Telefone, dto.Email, dto.Cnpj, dto.Responsavel);
            var filialAtualizada = await _atualizarUseCase.Executar(filial);
            
            if (filialAtualizada == null)
                return null;

            return FilialDTO.Criar(filialAtualizada);
        }

        public async Task<bool> Remover(Guid id)
        {
            var filial = await _buscarPorIdUseCase.Executar(id);
            
            // Se o BuscarPorIdUseCase retornou null, ele já notificou o erro
            if (filial == null)
                return false;

            return await _removerUseCase.Executar(filial);
        }

        public async Task<FilialDTO> BuscarPorId(Guid id)
        {
            var filial = await _buscarPorIdUseCase.Executar(id);
            
            // Se o BuscarPorIdUseCase retornou null, ele já notificou o erro
            if (filial == null)
                return null;

            return FilialDTO.Criar(filial);
        }

        public async Task<IEnumerable<FilialDTO>> Listar(QueryParamsBase query)
        {
            var filiais = await _listarUseCase.Executar(query);
            return filiais.Select(FilialDTO.Criar);
        }

        public async Task<bool> Ativar(Guid id)
        {
            var filial = await _buscarPorIdUseCase.Executar(id);
            
            // Se o BuscarPorIdUseCase retornou null, ele já notificou o erro
            if (filial == null)
                return false;

            filial.Ativar();
            var filialAtualizada = await _atualizarUseCase.Executar(filial);
            
            return filialAtualizada != null;
        }

        public async Task<bool> Desativar(Guid id)
        {
            var filial = await _buscarPorIdUseCase.Executar(id);
            
            // Se o BuscarPorIdUseCase retornou null, ele já notificou o erro
            if (filial == null)
                return false;

            filial.Desativar();
            var filialAtualizada = await _atualizarUseCase.Executar(filial);
            
            return filialAtualizada != null;
        }
    }
} 
