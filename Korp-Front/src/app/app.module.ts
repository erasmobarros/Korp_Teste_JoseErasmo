import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { provideHttpClient, withFetch } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ProdutoListComponent } from './components/produto-list/produto-list.component';
import { NotaFiscalComponent } from './components/nota-fiscal/nota-fiscal.component';



// --- ADICIONE ESTAS DUAS IMPORTAÇÕES ---
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
// ---------------------------------------

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ProdutoListComponent, // Seu componente standalone
    NotaFiscalComponent, 
    MatToolbarModule,     // <-- ADICIONE AQUI
    MatButtonModule       // <-- ADICIONE AQUI
  ],
  providers: [
    provideHttpClient(withFetch())
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }