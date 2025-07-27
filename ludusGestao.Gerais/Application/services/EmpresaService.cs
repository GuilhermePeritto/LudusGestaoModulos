using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Notificacao;
using ludusGestao.Gerais.Domain.Empresa.DTOs;
using ludusGestao.Gerais.Domain.Empresa.Interfaces;
using ludusGestao.Gerais.Domain.Empresa;

namespace ludusGestao.Gerais.Application.Services
{
    public class EmpresaService : BaseService, IEmpresaService
    {
        private readonly ICriarEmpresaUseCase _criarUseCase;
        private readonly IAtualizarEmpresaUseCase _atualizarUseCase;
        private readonly IRemoverEmpresaUseCase _removerUseCase;
        private readonly IBuscarEmpresaPorIdUseCase _buscarPorIdUseCase;
        private readonly IListarEmpresasUseCase _listarUseCase;

        public EmpresaService(
            ICriarEmpresaUseCase criarUseCase,
            IAtualizarEmpresaUseCase atualizarUseCase,
            IRemoverEmpresaUseCase removerUseCase,
            IBuscarEmpresaPorIdUseCase buscarPorIdUseCase,
            IListarEmpresasUseCase listarUseCase,
            INotificador notificador)
            : base(notificador)
        {
            _criarUseCase = criarUseCase;
            _atualizarUseCase = atualizarUseCase;
            _removerUseCase = removerUseCase;
            _buscarPorIdUseCase = buscarPorIdUseCase;
            _listarUseCase = listarUseCase;
        }

        public async Task<EmpresaDTO> Criar(CriarEmpresaDTO dto)
        {
            var empresa = Empresa.Criar(dto.Nome, dto.Cnpj, dto.Email, dto.Rua, dto.Numero, dto.Bairro, dto.Cidade, dto.Estado, dto.Cep, dto.Telefone);
            var empresaCriada = await _criarUseCase.Executar(empresa);
            
            if (empresaCriada == null)
                return null;

            return EmpresaDTO.Criar(empresaCriada);
        }

        public async Task<EmpresaDTO> Atualizar(Guid id, AtualizarEmpresaDTO dto)
        {
            var empresa = await _buscarPorIdUseCase.Executar(id);
            
            if (empresa == null)
                return null;

            empresa.Atualizar(dto.Nome, dto.Cnpj, dto.Email, dto.Rua, dto.Numero, dto.Bairro, dto.Cidade, dto.Estado, dto.Cep, dto.Telefone);
            var empresaAtualizada = await _atualizarUseCase.Executar(empresa);
            
            if (empresaAtualizada == null)
                return null;

            return EmpresaDTO.Criar(empresaAtualizada);
        }

        public async Task<bool> Remover(Guid id)
        {
            var empresa = await _buscarPorIdUseCase.Executar(id);
            
            if (empresa == null)
                return false;

            return await _removerUseCase.Executar(empresa);
        }

        public async Task<EmpresaDTO> BuscarPorId(Guid id)
        {
            var empresa = await _buscarPorIdUseCase.Executar(id);
            
            if (empresa == null)
                return null;

            return EmpresaDTO.Criar(empresa);
        }

        public async Task<IEnumerable<EmpresaDTO>> Listar(QueryParamsBase query)
        {
            var empresas = await _listarUseCase.Executar(query);
            return empresas.Select(EmpresaDTO.Criar);
        }

        public async Task<bool> Ativar(Guid id)
        {
            var empresa = await _buscarPorIdUseCase.Executar(id);
            
            if (empresa == null)
                return false;

            empresa.Ativar();
            var empresaAtualizada = await _atualizarUseCase.Executar(empresa);
            
            return empresaAtualizada != null;
        }

        public async Task<bool> Desativar(Guid id)
        {
            var empresa = await _buscarPorIdUseCase.Executar(id);
            
            if (empresa == null)
                return false;

            empresa.Desativar();
            var empresaAtualizada = await _atualizarUseCase.Executar(empresa);
            
            return empresaAtualizada != null;
        }
    }
} 
