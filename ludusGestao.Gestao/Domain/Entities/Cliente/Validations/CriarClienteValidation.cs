using FluentValidation;
using ludusGestao.Gestao.Domain.Entities.Cliente.DTOs;

namespace ludusGestao.Gestao.Domain.Entities.Cliente.Validations
{
    public class CriarClienteValidation : AbstractValidator<CriarClienteDTO>
    {
        public CriarClienteValidation()
        {
            RuleFor(x => x.NomeEmpresa)
                .NotEmpty().WithMessage("Nome da empresa é obrigatório")
                .MaximumLength(100).WithMessage("Nome da empresa deve ter no máximo 100 caracteres");

            RuleFor(x => x.EmailEmpresa)
                .NotEmpty().WithMessage("Email da empresa é obrigatório")
                .EmailAddress().WithMessage("Email da empresa deve ser válido");

            RuleFor(x => x.TelefoneEmpresa)
                .NotEmpty().WithMessage("Telefone da empresa é obrigatório")
                .Matches(@"^\d{10,11}$").WithMessage("Telefone da empresa deve ter 10 ou 11 dígitos");

            RuleFor(x => x.CnpjEmpresa)
                .NotEmpty().WithMessage("CNPJ da empresa é obrigatório")
                .Matches(@"^\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}$").WithMessage("CNPJ da empresa deve estar no formato XX.XXX.XXX/XXXX-XX");

            RuleFor(x => x.RuaEmpresa)
                .NotEmpty().WithMessage("Rua da empresa é obrigatória");

            RuleFor(x => x.NumeroEmpresa)
                .NotEmpty().WithMessage("Número da empresa é obrigatório");

            RuleFor(x => x.BairroEmpresa)
                .NotEmpty().WithMessage("Bairro da empresa é obrigatório");

            RuleFor(x => x.CidadeEmpresa)
                .NotEmpty().WithMessage("Cidade da empresa é obrigatória");

            RuleFor(x => x.EstadoEmpresa)
                .NotEmpty().WithMessage("Estado da empresa é obrigatório")
                .Length(2).WithMessage("Estado deve ter 2 caracteres");

                                    RuleFor(x => x.CepEmpresa)
                            .NotEmpty().WithMessage("CEP da empresa é obrigatório")
                            .Matches(@"^\d{5}-\d{3}$").WithMessage("CEP deve estar no formato XXXXX-XXX");

                        RuleFor(x => x.ResponsavelFilial)
                            .NotEmpty().WithMessage("Responsável da filial é obrigatório");

                        RuleFor(x => x.DataAberturaFilial)
                            .NotEmpty().WithMessage("Data de abertura da filial é obrigatória")
                            .LessThanOrEqualTo(DateTime.Today).WithMessage("Data de abertura não pode ser futura");
        }
    }
} 