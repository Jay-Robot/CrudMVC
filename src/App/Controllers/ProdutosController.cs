using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using App.ViewModels;
using Business.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using Business.Models;

namespace App.Controllers
{
    public class ProdutosController : BaseController
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IMapper _mapper;
        public ProdutosController(IProdutoRepository produtoRepository, IFornecedorRepository fornecedorRepository, IMapper mapper)
        {
            _produtoRepository = produtoRepository;
            _fornecedorRepository = fornecedorRepository;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {            
            return View(_mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterProdutosFornecedores()));
        }

        public async Task<IActionResult> Details(Guid id)
        {

            var produtoViewModel = await ObterProduto(id);
            if (produtoViewModel == null)
            {
                return NotFound();
            }

            return View(produtoViewModel);
        }

        public async Task<IActionResult> Create()
        {
            ProdutoViewModel produto = await PopularFornecedores(new ProdutoViewModel());
            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProdutoViewModel produtoViewModel)
        {
            produtoViewModel = await PopularFornecedores(new ProdutoViewModel());

            if (ModelState.IsValid)
                return View(produtoViewModel);

            await _produtoRepository.Adicionar(_mapper.Map<Produto>(produtoViewModel));

            return View(produtoViewModel);
            
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);
            if (produtoViewModel == null)
                return NotFound();

            return View(produtoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProdutoViewModel produtoViewModel)
        {
            if (id != produtoViewModel.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(produtoViewModel);

            await _produtoRepository.Atualizar(_mapper.Map<Produto>(produtoViewModel));               
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);
            if (produtoViewModel == null)
            {
                return NotFound();
            }

            return View(produtoViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);
            if (produtoViewModel == null)
            {
                return NotFound();
            }

            await _produtoRepository.Remover(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task<ProdutoViewModel> ObterProduto(Guid id)
        {
            var produto = _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterProdutoFonecedor(id));
            produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());

            return produto;
        }

        private async Task<ProdutoViewModel> PopularFornecedores(ProdutoViewModel produto)
        {
            produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
            return produto;
        }
    }
}
