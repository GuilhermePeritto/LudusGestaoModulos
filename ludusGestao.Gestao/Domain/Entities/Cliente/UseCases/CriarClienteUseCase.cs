using System;
using System.Linq;
using System.Threading.Tasks;
using ludusGestao.Gestao.Domain.Entities.Cliente.DTOs;
using ludusGestao.Gestao.Domain.Entities.Cliente.Interfaces;
using ludusGestao.Gestao.Domain.Entities.Cliente.Validations;
using ludusGestao.Gerais.Domain.Empresa;
using ludusGestao.Gerais.Domain.Filial;
using ludusGestao.Gerais.Domain.Usuario;
using ludusGestao.Provider.Data.Contexts;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Notificacao;
using LudusGestao.Shared.Security;
using Microsoft.EntityFrameworkCore;

namespace ludusGestao.Gestao.Domain.Entities.Cliente.UseCases
{
    public class CriarClienteUseCase : BaseUseCase, ICriarClienteUseCase
    {
        private readonly LudusGestaoWriteDbContext _context;
        private readonly IPasswordHelper _passwordHelper;

        public CriarClienteUseCase(LudusGestaoWriteDbContext context, IPasswordHelper passwordHelper, INotificador notificador)
            : base(notificador)
        {
            _context = context;
            _passwordHelper = passwordHelper;
        }

        public async Task<ClienteCriadoDTO> Executar(CriarClienteDTO dto)
        {
            if (!ExecutarValidacao(new CriarClienteValidation(), dto))
                return null!;

            // Gerar novo TenantId (maior + 1)
            var maiorTenantId = await _context.Empresas
                .Select(e => e.TenantId)
                .DefaultIfEmpty(0)
                .MaxAsync();
            
            var novoTenantId = maiorTenantId + 1;

            // Gerar senha aleatória
            var senhaAleatoria = GerarSenhaAleatoria();
            var emailAdministrador = $"administrador@{dto.NomeEmpresa.ToLower().Replace(" ", "").Replace(".", "").Replace(",", "")}.com.br";

            // Criar Empresa
            var empresa = Empresa.Criar(
                nome: dto.NomeEmpresa,
                email: dto.EmailEmpresa,
                telefone: dto.TelefoneEmpresa,
                cnpj: dto.CnpjEmpresa,
                rua: dto.RuaEmpresa,
                numero: dto.NumeroEmpresa,
                bairro: dto.BairroEmpresa,
                cidade: dto.CidadeEmpresa,
                estado: dto.EstadoEmpresa,
                cep: dto.CepEmpresa
            );

            // Definir TenantId da empresa
            empresa.AlterarTenant(novoTenantId);

            _context.Empresas.Add(empresa);
            await _context.SaveChangesAsync();

                                    // Criar Filial (usando dados da empresa)
                        var filial = Filial.Criar(
                            nome: "Matriz",
                            rua: dto.RuaEmpresa,
                            numero: dto.NumeroEmpresa,
                            bairro: dto.BairroEmpresa,
                            cidade: dto.CidadeEmpresa,
                            estado: dto.EstadoEmpresa,
                            cep: dto.CepEmpresa,
                            telefone: dto.TelefoneEmpresa,
                            email: dto.EmailEmpresa,
                            cnpj: dto.CnpjEmpresa,
                            responsavel: dto.ResponsavelFilial,
                            dataAbertura: dto.DataAberturaFilial,
                            empresaId: empresa.Id
                        );

            // Definir TenantId da filial
            filial.AlterarTenant(novoTenantId);

            _context.Filiais.Add(filial);
            await _context.SaveChangesAsync();

            // Criar Usuário Administrador
            var usuario = Usuario.Criar(
                nome: "Administrador",
                email: emailAdministrador,
                telefone: dto.TelefoneEmpresa,
                cargo: "Administrador",
                empresaId: empresa.Id,
                senha: senhaAleatoria,
                rua: dto.RuaEmpresa,
                numero: dto.NumeroEmpresa,
                bairro: dto.BairroEmpresa,
                cidade: dto.CidadeEmpresa,
                estado: dto.EstadoEmpresa,
                cep: dto.CepEmpresa
            );

            // Definir TenantId do usuário
            usuario.AlterarTenant(novoTenantId);

            // Criptografar senha
            usuario.AlterarSenha(_passwordHelper.CriptografarSenha(senhaAleatoria));

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return new ClienteCriadoDTO
            {
                TenantId = novoTenantId,
                Empresa = ludusGestao.Gerais.Domain.Empresa.DTOs.EmpresaDTO.Criar(empresa),
                Filial = ludusGestao.Gerais.Domain.Filial.DTOs.FilialDTO.Criar(filial),
                Usuario = ludusGestao.Gerais.Domain.Usuario.DTOs.UsuarioDTO.Criar(usuario),
                SenhaAdministrador = senhaAleatoria,
                EmailAdministrador = emailAdministrador
            };
        }

        private string GerarSenhaAleatoria()
        {
            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            var random = new Random();
            var senha = new char[12];
            
            for (int i = 0; i < 12; i++)
            {
                senha[i] = caracteres[random.Next(caracteres.Length)];
            }
            
            return new string(senha);
        }
    }
} 