using Business.Interfaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
    {
        public FornecedorRepository(DataContext context): base(context)
        {

        }
        public async Task<Fornecedor> ObterFornecedorEndereco(Guid id)
        {
            return await Context.Fornecedores.AsNoTracking().Include(f => f.Endereco).FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Fornecedor> ObterFornecedorProdutosEndereco(Guid id)
        {
            return await Context.Fornecedores.AsNoTracking()
                .Include(f => f.Endereco)
                .Include(f => f.Produtos)
                .FirstOrDefaultAsync(f => f.Id == id);
        }
    }
}
