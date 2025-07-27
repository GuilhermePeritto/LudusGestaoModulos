using FluentValidation;
using ludusGestao.Eventos.Domain.Entities.Local.DTOs;

namespace ludusGestao.Eventos.Domain.Entities.Local.Validations
{
    public class CriarLocalValidation : AbstractValidator<CriarLocalDTO>
    {
        public CriarLocalValidation()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres");

            RuleFor(x => x.Descricao)
                .MaximumLength(500).WithMessage("Descrição deve ter no máximo 500 caracteres");

            RuleFor(x => x.Rua)
                .NotEmpty().WithMessage("Rua é obrigatória")
                .MaximumLength(100).WithMessage("Rua deve ter no máximo 100 caracteres");

            RuleFor(x => x.Numero)
                .NotEmpty().WithMessage("Número é obrigatório")
                .MaximumLength(10).WithMessage("Número deve ter no máximo 10 caracteres");

            RuleFor(x => x.Bairro)
                .NotEmpty().WithMessage("Bairro é obrigatório")
                .MaximumLength(50).WithMessage("Bairro deve ter no máximo 50 caracteres");

            RuleFor(x => x.Cidade)
                .NotEmpty().WithMessage("Cidade é obrigatória")
                .MaximumLength(50).WithMessage("Cidade deve ter no máximo 50 caracteres");

            RuleFor(x => x.Estado)
                .NotEmpty().WithMessage("Estado é obrigatório")
                .Length(2).WithMessage("Estado deve ter 2 caracteres");

            RuleFor(x => x.Cep)
                .NotEmpty().WithMessage("CEP é obrigatório")
                .Length(8).WithMessage("CEP deve ter 8 caracteres");

            RuleFor(x => x.Telefone)
                .NotEmpty().WithMessage("Telefone é obrigatório")
                .MaximumLength(15).WithMessage("Telefone deve ter no máximo 15 caracteres");
        }
    }
} 