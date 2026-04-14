import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

// Importações do Angular Material
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon'; 
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatCardModule } from '@angular/material/card'; // <--- Adicionado o MatCardModule aqui!

import { ProdutoService } from '../../services/produto.service';
import { Produto } from '../../models/produto.model';

@Component({
  selector: 'app-produto-list',
  standalone: true,
  templateUrl: './produto-list.component.html',
  styleUrls: ['./produto-list.component.css'],
  imports: [
    CommonModule, 
    FormsModule, 
    HttpClientModule,
    MatTableModule, 
    MatFormFieldModule, 
    MatInputModule, 
    MatButtonModule,
    MatIconModule,       
    MatSnackBarModule,    
    MatCardModule        // <--- E registrado aqui dentro também!
  ]
})
export class ProdutoListComponent implements OnInit {
  produtos: Produto[] = [];
  novoProduto: Produto = {descricao: '', saldo: 0 };
  colunas: string[] = ['id', 'descricao', 'saldo', 'acoes'];

  constructor(
    private produtoService: ProdutoService,
    private cdr: ChangeDetectorRef,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.carregarProdutos();
  }

  carregarProdutos() {
    this.produtoService.listar().subscribe({
      next: (dados) => {
        this.produtos = dados;
        this.cdr.detectChanges();
      }
    });
  }

  // Função para mostrar a mensagem de sucesso
  private mostrarSucesso(mensagem: string) {
    this.snackBar.open(mensagem, 'Fechar', {
      duration: 3000,
      horizontalPosition: 'right',
      verticalPosition: 'top'
    });
  }

 cadastrar() {
    // 1. Rastro para vermos no F12 se o botão pelo menos foi clicado
    console.log('Botão Cadastrar clicado! Dados na memória:', this.novoProduto);

    // 2. Validação com aviso na tela
    if (!this.novoProduto.descricao || this.novoProduto.descricao.trim() === '') {
      this.snackBar.open('Atenção: A descrição do produto é obrigatória!', 'OK', { duration: 3000 });
      return; // Para a função aqui
    }

    if (this.novoProduto.saldo == null || this.novoProduto.saldo < 0) {
      this.snackBar.open('Atenção: O saldo não pode ser negativo ou vazio!', 'OK', { duration: 3000 });
      return; // Para a função aqui
    }

    // 3. Comunicação com o servidor (Agora pegando erros também)
    this.produtoService.salvar(this.novoProduto).subscribe({
      next: () => {
        this.mostrarSucesso('Produto cadastrado com sucesso!');
        this.carregarProdutos();
        this.novoProduto = { descricao: '', saldo: 0 };
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Erro no servidor ao salvar:', err);
        this.snackBar.open('Erro ao salvar no banco. Veja o console (F12).', 'Fechar', { duration: 4000 });
      }
    });
  }

  // Função para excluir
  excluir(id: number | undefined) {
    if (!id) return;

    if (confirm('Tem certeza que deseja excluir este produto?')) {
      this.produtoService.excluir(id).subscribe({
        next: () => {
          this.mostrarSucesso('Produto excluído!');
          this.carregarProdutos();
          this.cdr.detectChanges();
        },
        error: (err) => {
          console.error(err);
          this.snackBar.open('Erro ao excluir', 'Fechar', { duration: 3000 });
        }
      });
    }
  }
}