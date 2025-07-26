using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao;

namespace ludusGestao.Provider.Data.Configurations.Autenticacao
{
    public class UsuarioAutenticacaoConfiguration : IEntityTypeConfiguration<UsuarioAutenticacao>
    {
        public void Configure(EntityTypeBuilder<UsuarioAutenticacao> builder)
        {
            builder.ToTable("UsuariosAutenticacao");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(u => u.Senha)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.Ativo)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(u => u.TenantId)
                .IsRequired();

            // Índices para melhor performance
            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.HasIndex(u => u.Ativo);

            // Configurações de auditoria
            builder.Property(u => u.DataCriacao)
                .IsRequired();

            builder.Property(u => u.DataAlteracao);
        }
    }
} 