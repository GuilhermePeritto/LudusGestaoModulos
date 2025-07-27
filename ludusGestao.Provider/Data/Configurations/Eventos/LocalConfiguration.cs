using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ludusGestao.Eventos.Domain.Entities.Local;

namespace ludusGestao.Provider.Data.Configurations.Eventos
{
    public class LocalConfiguration : IEntityTypeConfiguration<Local>
    {
        public void Configure(EntityTypeBuilder<Local> builder)
        {
            builder.ToTable("Locais");
            builder.HasKey(l => l.Id);
            
            builder.Property(l => l.Nome)
                .IsRequired()
                .HasMaxLength(200);

            // Configuração do Value Object Endereco
            builder.OwnsOne(l => l.Endereco, endereco =>
            {
                endereco.Property(e => e.Rua)
                    .HasColumnName("Rua")
                    .HasMaxLength(200)
                    .IsRequired();
                endereco.Property(e => e.Numero)
                    .HasColumnName("Numero")
                    .HasMaxLength(20)
                    .IsRequired();
                endereco.Property(e => e.Bairro)
                    .HasColumnName("Bairro")
                    .HasMaxLength(100)
                    .IsRequired();
                endereco.Property(e => e.Cidade)
                    .HasColumnName("Cidade")
                    .HasMaxLength(100)
                    .IsRequired();
                endereco.Property(e => e.Estado)
                    .HasColumnName("Estado")
                    .HasMaxLength(2)
                    .IsRequired();
                endereco.Property(e => e.Cep)
                    .HasColumnName("Cep")
                    .HasMaxLength(10)
                    .IsRequired();
            });

            // Configuração do Value Object Telefone
            builder.OwnsOne(l => l.Telefone, telefone =>
            {
                telefone.Property(t => t.Numero)
                    .HasColumnName("Telefone")
                    .HasMaxLength(20)
                    .IsRequired();
            });
        }
    }
} 