import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Produto } from '../models/produto.model';

@Injectable({
  providedIn: 'root'
})
export class ProdutoService {
  // Porta 5121 = EstoqueService (C#)
  private apiUrl = 'http://localhost:5121/api/produtos'; 

  constructor(private http: HttpClient) { }

  // Busca todos os produtos para a tabela
  listar(): Observable<Produto[]> {
    return this.http.get<Produto[]>(this.apiUrl);
  }

  // Salva um novo produto no banco SQLite
  salvar(produto: Produto): Observable<Produto> {
    return this.http.post<Produto>(this.apiUrl, produto);
  }

  // Remove um produto pelo ID
  excluir(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}