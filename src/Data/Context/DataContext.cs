using Business.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options): base(options)
        {

        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /* Caso alguma propriedade do tipo string não tenha sido declarada como varchar, faz isso automaticamente */
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(e=> e.GetProperties().Where(p=> p.ClrType == typeof(string))))
            {
                property.SetColumnType("varchar(100)");
            }
            
            /* Busca as entidades que estão mapeadas no contexto e busca classes que herdam de IEntityTypeConfiguration 
               para as entidades relacionadas no contexto e registra todas de uma vez só - via reflection     */
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

            /* Desabilitando a deleção de vários entidades relacionadas */
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
            }

            base.OnModelCreating(modelBuilder); 
        }

    }
}
