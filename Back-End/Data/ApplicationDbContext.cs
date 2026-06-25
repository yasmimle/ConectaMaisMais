using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    // Contexto principal do Entity Framework Core
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Representa as tabelas do banco de dados
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Interesse> Interesses => Set<Interesse>();
        public DbSet<Admin> Administradores => Set<Admin>();
        public DbSet<Atendente> Atendentes => Set<Atendente>();
        public DbSet<Ong> Ongs => Set<Ong>();
        public DbSet<Uf> Ufs => Set<Uf>();
        public DbSet<StatusInteresse> StatusInteresses => Set<StatusInteresse>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapeia tabela de unidades federativas
            modelBuilder.Entity<Uf>(entity =>
            {
                entity.ToTable("UF");
                entity.HasKey(e => e.IdUf);
                entity.Property(e => e.IdUf).HasColumnName("ID_UF");
                entity.Property(e => e.Sigla).HasColumnName("SIGLA_UF").HasMaxLength(2).IsRequired();
                entity.Property(e => e.Nome).HasColumnName("DSC_NOME").HasMaxLength(50).IsRequired();
                entity.HasIndex(e => e.Sigla).IsUnique();
            });

            // Mapeia tabela de status permitidos para interesses
            modelBuilder.Entity<StatusInteresse>(entity =>
            {
                entity.ToTable("STATUS");
                entity.HasKey(e => e.IdStatus);
                entity.Property(e => e.IdStatus).HasColumnName("ID_STATUS");
                entity.Property(e => e.Descricao).HasColumnName("DSC_STATUS").HasMaxLength(50).IsRequired();
                entity.HasIndex(e => e.Descricao).IsUnique();
            });

            // Mapeia dados da ONG e seus relacionamentos
            modelBuilder.Entity<Ong>(entity =>
            {
                entity.ToTable("ONG");
                entity.HasKey(e => e.IdOng);
                entity.Property(e => e.IdOng).HasColumnName("ID_ONG");
                entity.Property(e => e.Nome).HasColumnName("DSC_NOME").HasMaxLength(150).IsRequired();
                entity.Property(e => e.Cnpj).HasColumnName("NUM_CNPJ").HasMaxLength(14).IsRequired();
                entity.Property(e => e.Telefone).HasColumnName("NUM_TELEFONE").HasMaxLength(11);
                entity.Property(e => e.Email).HasColumnName("DSC_EMAIL").HasMaxLength(150);
                entity.Property(e => e.Endereco).HasColumnName("DSC_ENDERECO").HasMaxLength(200);
                entity.HasIndex(e => e.Cnpj).IsUnique();
                entity.HasMany(e => e.Administradores)
                    .WithOne(e => e.Ong)
                    .HasForeignKey(e => e.IdOng)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(e => e.Atendentes)
                    .WithOne(e => e.Ong)
                    .HasForeignKey(e => e.IdOng)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Mapeia usuários públicos cadastrados
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("USUARIO");
                entity.HasKey(e => e.IdUsuario);
                entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");
                entity.Property(e => e.IdUf).HasColumnName("ID_UF");
                entity.Property(e => e.Nome).HasColumnName("DSC_NOME").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Cpf).HasColumnName("NUM_CPF").HasMaxLength(11).IsRequired();
                entity.Property(e => e.Telefone).HasColumnName("NUM_TELEFONE").HasMaxLength(11);
                entity.Property(e => e.Email).HasColumnName("DSC_EMAIL").HasMaxLength(150);
                entity.Property(e => e.Endereco).HasColumnName("DSC_ENDERECO").HasMaxLength(200).IsRequired();
                entity.Property(e => e.Cidade).HasColumnName("DSC_CIDADE").HasMaxLength(100).IsRequired();
                entity.Property(e => e.DataNascimento).HasColumnName("DATA_NASCIMENTO").HasColumnType("date");
                entity.Property(e => e.DataCadastro).HasColumnName("DATA_CADASTRO").HasColumnType("datetime2");
                entity.Property(e => e.TipoParticipacao).HasColumnName("DSC_TIPO_PARTICIPACAO").HasMaxLength(20).IsRequired();
                entity.HasIndex(e => e.Cpf).IsUnique();
                entity.HasOne(e => e.Uf)
                    .WithMany(e => e.Usuarios)
                    .HasForeignKey(e => e.IdUf)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Mapeia solicitações/interesses vinculados aos usuários
            modelBuilder.Entity<Interesse>(entity =>
            {
                entity.ToTable("INTERESSE");
                entity.HasKey(e => e.IdInteresse);
                entity.Property(e => e.IdInteresse).HasColumnName("ID_INTERESSE");
                entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");
                entity.Property(e => e.IdStatus).HasColumnName("ID_STATUS");
                entity.Property(e => e.Tipo).HasColumnName("DSC_TIPO").HasMaxLength(20).IsRequired();
                entity.Property(e => e.Descricao).HasColumnName("DSC_DESCRICAO").HasMaxLength(500).IsRequired();
                entity.Property(e => e.DataRegistro).HasColumnName("DATA_REGISTRO").HasColumnType("datetime2");
                entity.HasOne(e => e.Usuario)
                    .WithMany(e => e.Interesses)
                    .HasForeignKey(e => e.IdUsuario)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Status)
                    .WithMany(e => e.Interesses)
                    .HasForeignKey(e => e.IdStatus)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Mapeia administradores do sistema
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("ADMIN");
                entity.HasKey(e => e.IdAdmin);
                entity.Property(e => e.IdAdmin).HasColumnName("ID_ADMIN");
                entity.Property(e => e.IdOng).HasColumnName("ID_ONG");
                entity.Property(e => e.Nome).HasColumnName("DSC_NOME").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Email).HasColumnName("DSC_EMAIL").HasMaxLength(150).IsRequired();
                entity.Property(e => e.Telefone).HasColumnName("NUM_TELEFONE").HasMaxLength(11);
                entity.Property(e => e.SenhaHash).HasColumnName("DSC_SENHA_HASH").HasMaxLength(256).IsRequired();
                entity.Property(e => e.SenhaSalt).HasColumnName("DSC_SENHA_SALT").HasMaxLength(128).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasOne(e => e.Ong)
                    .WithMany(e => e.Administradores)
                    .HasForeignKey(e => e.IdOng)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Mapeia atendentes do sistema
            modelBuilder.Entity<Atendente>(entity =>
            {
                entity.ToTable("ATENDENTE");
                entity.HasKey(e => e.IdAtendente);
                entity.Property(e => e.IdAtendente).HasColumnName("ID_ATENDENTE");
                entity.Property(e => e.IdOng).HasColumnName("ID_ONG");
                entity.Property(e => e.Nome).HasColumnName("DSC_NOME").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Email).HasColumnName("DSC_EMAIL").HasMaxLength(150).IsRequired();
                entity.Property(e => e.Telefone).HasColumnName("NUM_TELEFONE").HasMaxLength(11);
                entity.Property(e => e.SenhaHash).HasColumnName("DSC_SENHA_HASH").HasMaxLength(256).IsRequired();
                entity.Property(e => e.SenhaSalt).HasColumnName("DSC_SENHA_SALT").HasMaxLength(128).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasOne(e => e.Ong)
                    .WithMany(e => e.Atendentes)
                    .HasForeignKey(e => e.IdOng)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
