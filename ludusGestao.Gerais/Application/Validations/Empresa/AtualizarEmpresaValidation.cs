using FluentValidation;
using ludusGestao.Gerais.Domain.DTOs.Empresa;

namespace ludusGestao.Gerais.Application.Validations.Empresa
{
    public class AtualizarEmpresaValidation : AbstractValidator<AtualizarEmpresaDTO>
    {
        public AtualizarEmpresaValidation()
        {
            RuleFor(x => x.Nome).NotEmpty().WithMessage("Nome é obrigatório.");
            RuleFor(x => x.Cnpj).NotNull().WithMessage("CNPJ é obrigatório.");
            RuleFor(x => x.Cnpj.Valor).NotEmpty().WithMessage("CNPJ inválido.");
            RuleFor(x => x.Endereco).NotNull().WithMessage("Endereço é obrigatório.");
            RuleFor(x => x.Endereco.Rua).NotEmpty().WithMessage("Rua é obrigatória.");
            RuleFor(x => x.Endereco.Numero).NotEmpty().WithMessage("Número é obrigatório.");
            RuleFor(x => x.Endereco.Bairro).NotEmpty().WithMessage("Bairro é obrigatório.");
            RuleFor(x => x.Endereco.Cidade).NotEmpty().WithMessage("Cidade é obrigatória.");
            RuleFor(x => x.Endereco.Estado).NotEmpty().WithMessage("Estado é obrigatório.");
            RuleFor(x => x.Endereco.Cep).NotEmpty().WithMessage("CEP é obrigatório.");
        }
    }
} 