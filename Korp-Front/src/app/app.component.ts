import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  standalone: false // <-- ADICIONE ESTA LINHA AQUI
})
export class AppComponent {
  title = 'Korp-Front';
}