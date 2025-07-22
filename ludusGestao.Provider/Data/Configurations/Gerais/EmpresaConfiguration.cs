using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ludusGestao.Gerais.Domain.Entities;

namespace ludusGestao.Provider.Data.Configurations.Gerais
{
    public class EmpresaConfiguration : IEntityTypeConfiguration<Empresa>
    {
        public void Configure(EntityTypeBuilder<Empresa> builder)
        {
            builder.ToTable("Empresas");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Nome).IsRequired().HasMaxLength(200);
            builder.Property(e => e.Telefone).HasMaxLength(20);

            builder.OwnsOne(e => e.Cnpj, cnpj =>
            {
                cnpj.Property(c => c.Valor).HasColumnName("Cnpj").IsRequired();
            });

            builder.OwnsOne(e => e.Endereco, endereco =>
            {
                endereco.Property(e => e.Rua).HasColumnName("Rua").HasMaxLength(200).IsRequired();
                endereco.Property(e => e.Numero).HasColumnName("Numero").HasMaxLength(20).IsRequired();
                endereco.Property(e => e.Bairro).HasColumnName("Bairro").HasMaxLength(100).IsRequired();
                endereco.Property(e => e.Cidade).HasColumnName("Cidade").HasMaxLength(100).IsRequired();
                endereco.Property(e => e.Estado).HasColumnName("Estado").HasMaxLength(2).IsRequired();
                endereco.Property(e => e.Cep).HasColumnName("Cep").HasMaxLength(10).IsRequired();
            });
        }
    }
} 