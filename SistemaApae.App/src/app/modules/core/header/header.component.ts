import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.less'],
})
export class HeaderComponent {
  userName: string = 'Luana Marini';
  userRole: string = 'Coordenador';
  userInitials: string = 'LM';
  pageTitle: string = 'Dashboard';
  pageSubtitle: string = 'Sistema de Gestão de Atendimentos';

  constructor(private router: Router) {}

  logout(): void {
    // Implementar lógica de logout
    console.log('Logout clicked');
    // this.router.navigate(['/login']);
  }

  onProfileClick(): void {
    // Implementar navegação para perfil do usuário
    console.log('Profile clicked');
  }
}
