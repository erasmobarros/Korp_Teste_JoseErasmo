import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon'; 


import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { MatCardModule } from '@angular/material/card';           
import { MatFormFieldModule } from '@angular/material/form-field'; 
import { MatInputModule } from '@angular/material/input';         


import { NotaFiscalService } from '../../services/nota-fiscal.service';
import { ProdutoService } from '../../services/produto.service';
import { Produto } from '../../models/produto.model';
import { NotaFiscal } from '../../models/nota-fiscal.model';

@Component({
  selector: 'app-nota-fiscal',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule, 
    MatTableModule, 
    MatSnackBarModule, 
    MatButtonModule, 
    MatSelectModule, 
    MatProgressSpinnerModule,
    MatCardModule,      
    MatFormFieldModule, 
    MatInputModule,
    MatIconModule 
  ],
  templateUrl: './nota-fiscal.component.html'
})
export class NotaFiscalComponent implements OnInit {
  notas: NotaFiscal[] = [];
  produtosDisponiveis: Produto[] = [];
  carregando: boolean = false;
  
  colunas: string[] = ['id', 'clienteNome', 'status', 'produtos', 'acoes'];
  
  novaNota: any = { clienteNome: '', itens: [] };
  itemTemp: any = { produtoId: 0, quantidade: 1 };

  constructor(
    private notaService: NotaFiscalService,
    private produtoService: ProdutoService,
    private cdr: ChangeDetectorRef,
    private snackBar: MatSnackBar,
  ) { }

  ngOnInit() {
    this.carregarNotas();
    this.produtoService.listar().subscribe(p => {
      this.produtosDisponiveis = p;
      this.cdr.detectChanges();
    });
  }

  carregarNotas() {
    this.notaService.listar().subscribe(n => { 
      this.notas = n; 
      this.cdr.detectChanges(); 
    });
  }

  adicionarItem() {
    const idSelecionado = Number(this.itemTemp.produtoId);
    const prod = this.produtosDisponiveis.find(p => p.id === idSelecionado);

    console.log('Tentando adicionar produto ID:', idSelecionado);
    
    if (prod) {
      this.novaNota.itens = [
        ...this.novaNota.itens, 
        { 
          produtoId: prod.id, 
          descricao: prod.descricao, 
          quantidade: this.itemTemp.quantidade 
        }
      ];

      this.itemTemp = { produtoId: 0, quantidade: 1 };
      this.cdr.detectChanges();
      
      this.snackBar.open('Produto adicionado à lista!', 'OK', { duration: 2000 });
    } else {
      this.snackBar.open('Selecione um produto válido!', 'Erro', { duration: 3000 });
    }
  }

  salvarNota() {
    if (!this.novaNota.clienteNome || this.novaNota.itens.length === 0) {
      this.snackBar.open('Informe o cliente e adicione ao menos um produto!', 'OK', { duration: 3000 });
      return;
    }

    this.notaService.gerar(this.novaNota).subscribe({
      next: (notaSalva) => {
        this.snackBar.open(`Nota nº ${notaSalva.id} gerada com sucesso!`, 'Fechar', { duration: 3000 });
        
        this.novaNota = { 
          clienteNome: '', 
          status: 'Aberta', 
          itens: [] 
        };
        
        this.carregarNotas();
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Erro ao salvar nota:', err);
        this.snackBar.open('Erro ao salvar nota no servidor.', 'Fechar', { duration: 3000 });
      }
    });
  }

  imprimir(id: number) {
    this.carregando = true;
    this.cdr.detectChanges(); 
    
    setTimeout(() => {
      this.notaService.imprimir(id).subscribe({
        next: () => {
          this.snackBar.open('✅ Impressão realizada e estoque baixado!', 'Sucesso', { duration: 3000 });
          this.carregando = false;
          this.carregarNotas();
          this.cdr.detectChanges(); 
        },
        error: (err) => {
          console.error('Erro detectado na comunicação com o Estoque:', err);
          
          this.snackBar.open(
            '❌ A nota NÃO foi fechada! A quantidade vendida é maior que o saldo no estoque (ou o serviço está offline).', 
            'Entendi', 
            { 
              duration: 6000, 
              horizontalPosition: 'center',
              verticalPosition: 'bottom'
            }
          );
          
          this.carregando = false;
          this.cdr.detectChanges(); 
        }
      });
    }, 1500);
  }

  
  excluirNota(id: number) {
    if (confirm('Tem certeza que deseja excluir esta nota definitivamente?')) {
      this.notaService.excluir(id).subscribe({
        next: () => {
          this.snackBar.open('Nota excluída com sucesso!', 'OK', { duration: 3000 });
          this.carregarNotas(); 
        },
        error: (err) => {
          console.error('Erro ao excluir nota:', err);
          this.snackBar.open('Erro ao excluir a nota.', 'Fechar', { duration: 3000 });
        }
      });
    }
  }
}