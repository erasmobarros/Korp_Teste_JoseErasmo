import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProdutoListComponent } from './components/produto-list/produto-list.component';
import { NotaFiscalComponent } from './components/nota-fiscal/nota-fiscal.component';

const routes: Routes = [
  { path: 'produtos', component: ProdutoListComponent },
  { path: 'notas', component: NotaFiscalComponent }, // CERTIFIQUE-SE QUE ESTÁ ASSIM
  { path: '', redirectTo: '/produtos', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }