using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ludusGestao.Gerais.Domain.Entities;

namespace ludusGestao.Provider.Data.Configurations.Gerais
{
    public class FilialConfiguration : IEntityTypeConfiguration<Filial>
    {
        public void Configure(EntityTypeBuilder<Filial> builder)
        {
            builder.ToTable("Filiais");
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Nome).IsRequired().HasMaxLength(200);
            builder.Property(f => f.Telefone).HasMaxLength(20);

            builder.OwnsOne(f => f.Cnpj, cnpj =>
            {
                cnpj.Property(c => c.Valor).HasColumnName("Cnpj").IsRequired();
            });

            builder.OwnsOne(f => f.Endereco, endereco =>
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