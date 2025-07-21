using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ludusGestao.Eventos.Domain.Entities;

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

            builder.Property(l => l.Capacidade)
                .IsRequired();

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

            // Índices para melhor performance
            builder.HasIndex(l => l.Nome);
            builder.HasIndex(l => l.Capacidade);

            // Configurações de auditoria
            builder.Property(l => l.DataCriacao)
                .IsRequired();
        }
    }
} 