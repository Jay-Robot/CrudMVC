using Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Mappings
{
    public class FornecedorMapping : IEntityTypeConfiguration<Fornecedor>
    {
        public void Configure(EntityTypeBuilder<Fornecedor> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Nome)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(f => f.Documento)
                .IsRequired()
                .HasColumnType("varchar(14)");


            builder.HasOne(f => f.Endereco)
                .WithOne(e => e.Fornecedor);

            builder.HasMany(f => f.Produtos)
                .WithOne(p => p.Fornecedor)
                .HasForeignKey(p => p.FornecedorId);


            builder.ToTable("Fornecedores");

        }
    }
}
