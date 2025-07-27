using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ludusGestao.Gerais.Domain.Usuario;

namespace ludusGestao.Provider.Data.Configurations.Gerais
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Nome).IsRequired().HasMaxLength(200);
            builder.Property(u => u.Telefone).HasMaxLength(20);

            builder.OwnsOne(u => u.Email, email =>
            {
                email.Property(e => e.Endereco).HasColumnName("Email").IsRequired();
            });

            builder.OwnsOne(u => u.Endereco, endereco =>
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