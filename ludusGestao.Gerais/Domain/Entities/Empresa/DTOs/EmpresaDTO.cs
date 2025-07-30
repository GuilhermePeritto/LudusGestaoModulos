using System;
using ludusGestao.Gerais.Domain.Empresa;
using LudusGestao.Shared.Domain.Entities;

namespace ludusGestao.Gerais.Domain.Empresa.DTOs
{
    public class EmpresaDTO : DTOBase
    {
        public string Nome { get; private set; }
        public string Cnpj { get; private set; }
        public string Email { get; private set; }
        public string Rua { get; private set; }
        public string Numero { get; private set; }
        public string Bairro { get; private set; }
        public string Cidade { get; private set; }
        public string Estado { get; private set; }
        public string Cep { get; private set; }
        public string Telefone { get; private set; }
        public string Situacao { get; private set; }

        // Construtor padrão necessário para o DTOBase
        public EmpresaDTO()
        {
        }

        // Método original mantido para compatibilidade
        public static EmpresaDTO Criar(Empresa empresa)
        {
            return new EmpresaDTO
            {
                Id = empresa.Id,
                Nome = empresa.Nome,
                Cnpj = empresa.Cnpj.Numero,
                Email = empresa.Email.Endereco,
                Rua = empresa.Endereco.Rua,
                Numero = empresa.Endereco.Numero,
                Bairro = empresa.Endereco.Bairro,
                Cidade = empresa.Endereco.Cidade,
                Estado = empresa.Endereco.Estado,
                Cep = empresa.Endereco.Cep,
                Telefone = empresa.Telefone.Numero,
                Situacao = empresa.Situacao.ToString(),
                TenantId = empresa.TenantId,
                DataCriacao = empresa.DataCriacao,
                DataAlteracao = empresa.DataAlteracao
            };
        }

        // ✅ SOBRECARGA: Aceita qualquer objeto (entidade completa ou parcial)
        public static EmpresaDTO Criar(object empresa)
        {
            // Se for uma entidade Empresa completa, usa o método original
            if (empresa is Empresa empresaCompleta)
            {
                return Criar(empresaCompleta);
            }
            
            // Se for um objeto parcial (com campos específicos), usa DTOBase
            return DTOBase.Criar<EmpresaDTO>(empresa);
        }
    }
} 