import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, MatIconModule],
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
    console.log('Logout clicked');
    // this.router.navigate(['/login']);
  }

  onProfileClick(): void {
    console.log('Profile clicked');
  }
}
