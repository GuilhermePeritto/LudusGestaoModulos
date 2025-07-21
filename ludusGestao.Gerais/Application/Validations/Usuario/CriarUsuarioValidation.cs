using FluentValidation;
using ludusGestao.Gerais.Domain.DTOs.Usuario;

namespace ludusGestao.Gerais.Application.Validations.Usuario
{
    public class CriarUsuarioValidation : AbstractValidator<CriarUsuarioDTO>
    {
        public CriarUsuarioValidation()
        {
            RuleFor(x => x.Nome).NotEmpty().WithMessage("Nome é obrigatório.");
            RuleFor(x => x.Email).NotNull().WithMessage("Email é obrigatório.");
            RuleFor(x => x.Email.Valor).NotEmpty().EmailAddress().WithMessage("Email inválido.");
            RuleFor(x => x.Senha).NotEmpty().MinimumLength(6).WithMessage("Senha deve ter pelo menos 6 caracteres.");
            RuleFor(x => x.EmpresaId).NotEmpty().WithMessage("Empresa é obrigatória.");
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