using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using WebApplication1.Data;

#nullable disable

namespace WebApplication1.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebApplication1.Models.Admin", b =>
            {
                b.Property<int>("IdAdmin")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasColumnName("ID_ADMIN");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdAdmin"));

                b.Property<string>("Email")
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnType("nvarchar(150)")
                    .HasColumnName("DSC_EMAIL");

                b.Property<int>("IdOng")
                    .HasColumnType("int")
                    .HasColumnName("ID_ONG");

                b.Property<string>("Nome")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)")
                    .HasColumnName("DSC_NOME");

                b.Property<string>("Telefone")
                    .HasMaxLength(11)
                    .HasColumnType("nvarchar(11)")
                    .HasColumnName("NUM_TELEFONE");

                b.HasKey("IdAdmin");
                b.HasIndex("Email").IsUnique();
                b.HasIndex("IdOng");
                b.ToTable("ADMIN", (string)null);
            });

            modelBuilder.Entity("WebApplication1.Models.Atendente", b =>
            {
                b.Property<int>("IdAtendente")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasColumnName("ID_ATENDENTE");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdAtendente"));

                b.Property<string>("Email")
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnType("nvarchar(150)")
                    .HasColumnName("DSC_EMAIL");

                b.Property<int>("IdOng")
                    .HasColumnType("int")
                    .HasColumnName("ID_ONG");

                b.Property<string>("Nome")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)")
                    .HasColumnName("DSC_NOME");

                b.Property<string>("SenhaHash")
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnType("nvarchar(256)")
                    .HasColumnName("DSC_SENHA_HASH");

                b.Property<string>("SenhaSalt")
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnType("nvarchar(128)")
                    .HasColumnName("DSC_SENHA_SALT");

                b.Property<string>("Telefone")
                    .HasMaxLength(11)
                    .HasColumnType("nvarchar(11)")
                    .HasColumnName("NUM_TELEFONE");

                b.HasKey("IdAtendente");
                b.HasIndex("Email").IsUnique();
                b.HasIndex("IdOng");
                b.ToTable("ATENDENTE", (string)null);
            });

            modelBuilder.Entity("WebApplication1.Models.Interesse", b =>
            {
                b.Property<int>("IdInteresse")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasColumnName("ID_INTERESSE");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdInteresse"));

                b.Property<DateTime>("DataRegistro")
                    .HasColumnType("datetime2")
                    .HasColumnName("DATA_REGISTRO");

                b.Property<string>("Descricao")
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnType("nvarchar(500)")
                    .HasColumnName("DSC_DESCRICAO");

                b.Property<int>("IdStatus")
                    .HasColumnType("int")
                    .HasColumnName("ID_STATUS");

                b.Property<int>("IdUsuario")
                    .HasColumnType("int")
                    .HasColumnName("ID_USUARIO");

                b.Property<string>("Tipo")
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnType("nvarchar(20)")
                    .HasColumnName("DSC_TIPO");

                b.HasKey("IdInteresse");
                b.HasIndex("IdStatus");
                b.HasIndex("IdUsuario");
                b.ToTable("INTERESSE", (string)null);
            });

            modelBuilder.Entity("WebApplication1.Models.Ong", b =>
            {
                b.Property<int>("IdOng")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasColumnName("ID_ONG");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdOng"));

                b.Property<string>("Cnpj")
                    .IsRequired()
                    .HasMaxLength(14)
                    .HasColumnType("nvarchar(14)")
                    .HasColumnName("NUM_CNPJ");

                b.Property<string>("Email")
                    .HasMaxLength(150)
                    .HasColumnType("nvarchar(150)")
                    .HasColumnName("DSC_EMAIL");

                b.Property<string>("Endereco")
                    .HasMaxLength(200)
                    .HasColumnType("nvarchar(200)")
                    .HasColumnName("DSC_ENDERECO");

                b.Property<string>("Nome")
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnType("nvarchar(150)")
                    .HasColumnName("DSC_NOME");

                b.Property<string>("Telefone")
                    .HasMaxLength(11)
                    .HasColumnType("nvarchar(11)")
                    .HasColumnName("NUM_TELEFONE");

                b.HasKey("IdOng");
                b.HasIndex("Cnpj").IsUnique();
                b.ToTable("ONG", (string)null);
            });

            modelBuilder.Entity("WebApplication1.Models.StatusInteresse", b =>
            {
                b.Property<int>("IdStatus")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasColumnName("ID_STATUS");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdStatus"));

                b.Property<string>("Descricao")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)")
                    .HasColumnName("DSC_STATUS");

                b.HasKey("IdStatus");
                b.HasIndex("Descricao").IsUnique();
                b.ToTable("STATUS", (string)null);
            });

            modelBuilder.Entity("WebApplication1.Models.Uf", b =>
            {
                b.Property<int>("IdUf")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasColumnName("ID_UF");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdUf"));

                b.Property<string>("Nome")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)")
                    .HasColumnName("DSC_NOME");

                b.Property<string>("Sigla")
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasColumnType("nvarchar(2)")
                    .HasColumnName("SIGLA_UF");

                b.HasKey("IdUf");
                b.HasIndex("Sigla").IsUnique();
                b.ToTable("UF", (string)null);
            });

            modelBuilder.Entity("WebApplication1.Models.Usuario", b =>
            {
                b.Property<int>("IdUsuario")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasColumnName("ID_USUARIO");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdUsuario"));

                b.Property<string>("Cidade")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)")
                    .HasColumnName("DSC_CIDADE");

                b.Property<string>("Cpf")
                    .IsRequired()
                    .HasMaxLength(11)
                    .HasColumnType("nvarchar(11)")
                    .HasColumnName("NUM_CPF");

                b.Property<DateTime>("DataCadastro")
                    .HasColumnType("datetime2")
                    .HasColumnName("DATA_CADASTRO");

                b.Property<DateTime>("DataNascimento")
                    .HasColumnType("date")
                    .HasColumnName("DATA_NASCIMENTO");

                b.Property<string>("Email")
                    .HasMaxLength(150)
                    .HasColumnType("nvarchar(150)")
                    .HasColumnName("DSC_EMAIL");

                b.Property<string>("Endereco")
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnType("nvarchar(200)")
                    .HasColumnName("DSC_ENDERECO");

                b.Property<int>("IdUf")
                    .HasColumnType("int")
                    .HasColumnName("ID_UF");

                b.Property<string>("Nome")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)")
                    .HasColumnName("DSC_NOME");

                b.Property<string>("SenhaHash")
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnType("nvarchar(256)")
                    .HasColumnName("DSC_SENHA_HASH");

                b.Property<string>("SenhaSalt")
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnType("nvarchar(128)")
                    .HasColumnName("DSC_SENHA_SALT");

                b.Property<string>("Telefone")
                    .HasMaxLength(11)
                    .HasColumnType("nvarchar(11)")
                    .HasColumnName("NUM_TELEFONE");

                b.Property<string>("TipoParticipacao")
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnType("nvarchar(20)")
                    .HasColumnName("DSC_TIPO_PARTICIPACAO");

                b.HasKey("IdUsuario");
                b.HasIndex("IdUf");
                b.HasIndex("Cpf").IsUnique();
                b.ToTable("USUARIO", (string)null);
            });

            modelBuilder.Entity("WebApplication1.Models.Admin", b =>
            {
                b.HasOne("WebApplication1.Models.Ong", "Ong")
                    .WithMany("Administradores")
                    .HasForeignKey("IdOng")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                b.Navigation("Ong");
            });

            modelBuilder.Entity("WebApplication1.Models.Atendente", b =>
            {
                b.HasOne("WebApplication1.Models.Ong", "Ong")
                    .WithMany("Atendentes")
                    .HasForeignKey("IdOng")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                b.Navigation("Ong");
            });

            modelBuilder.Entity("WebApplication1.Models.Interesse", b =>
            {
                b.HasOne("WebApplication1.Models.StatusInteresse", "Status")
                    .WithMany("Interesses")
                    .HasForeignKey("IdStatus")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                b.HasOne("WebApplication1.Models.Usuario", "Usuario")
                    .WithMany("Interesses")
                    .HasForeignKey("IdUsuario")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Status");
                b.Navigation("Usuario");
            });

            modelBuilder.Entity("WebApplication1.Models.Usuario", b =>
            {
                b.HasOne("WebApplication1.Models.Uf", "Uf")
                    .WithMany("Usuarios")
                    .HasForeignKey("IdUf")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                b.Navigation("Uf");
            });

            modelBuilder.Entity("WebApplication1.Models.Ong", b =>
            {
                b.Navigation("Administradores");
                b.Navigation("Atendentes");
            });

            modelBuilder.Entity("WebApplication1.Models.StatusInteresse", b =>
            {
                b.Navigation("Interesses");
            });

            modelBuilder.Entity("WebApplication1.Models.Uf", b =>
            {
                b.Navigation("Usuarios");
            });

            modelBuilder.Entity("WebApplication1.Models.Usuario", b =>
            {
                b.Navigation("Interesses");
            });
        }
    }
}
