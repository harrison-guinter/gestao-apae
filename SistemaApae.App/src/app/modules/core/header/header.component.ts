import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { Subscription } from 'rxjs';
import { PageInfoService, PageInfo } from '../services/page-info.service';
import { AuthService } from '../../auth/auth.service';
import { NotificationService } from '../notification/notification.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.less'],
})
export class HeaderComponent implements OnInit, OnDestroy {
  userName: string = 'Usuário';
  userRole: string = 'Perfil';
  userInitials: string = 'U';
  pageTitle: string = 'Dashboard';
  pageSubtitle: string = 'Sistema de Gestão de Atendimentos';

  private pageInfoSubscription?: Subscription;

  constructor(
    private router: Router,
    private pageInfoService: PageInfoService,
    private authService: AuthService,
    private notificationService: NotificationService
  ) {}

  ngOnInit() {
    this.loadUserInfo();

    this.pageInfoSubscription = this.pageInfoService.pageInfo$.subscribe((pageInfo: PageInfo) => {
      this.pageTitle = pageInfo.title;
      this.pageSubtitle = pageInfo.subtitle;
    });
  }

  private loadUserInfo(): void {
    try {
      const userString = localStorage.getItem('usuario');
      if (userString) {
        const user = JSON.parse(userString);
        this.userName = user.nome || 'Usuário';

        // Mapear perfil para string
        switch (user.perfil) {
          case 1:
            this.userRole = 'Coordenador';
            break;
          case 2:
            this.userRole = 'Profissional';
            break;
          default:
            this.userRole = 'Usuário';
        }

        // Gerar iniciais do nome
        const names = this.userName.split(' ');
        if (names.length >= 2) {
          this.userInitials = names[0][0] + names[1][0];
        } else if (names.length === 1) {
          this.userInitials = names[0][0] + names[0][1] || names[0][0];
        }
        this.userInitials = this.userInitials.toUpperCase();
      }
    } catch (error) {
      console.error('Erro ao carregar informações do usuário:', error);
    }
  }

  ngOnDestroy() {
    if (this.pageInfoSubscription) {
      this.pageInfoSubscription.unsubscribe();
    }
  }

  logout(): void {
    const confirmLogout = confirm('Tem certeza que deseja sair do sistema?');

    if (confirmLogout) {
      try {
        // Limpar dados do localStorage
        this.authService.logout();

        // Mostrar notificação de sucesso
        this.notificationService.success('Logout realizado com sucesso!');

        // Redirecionar para a página de login
        this.router.navigate(['/login']);
      } catch (error) {
        console.error('Erro durante logout:', error);
        this.notificationService.fail('Erro ao realizar logout');
      }
    }
  }

  onProfileClick(): void {
    console.log('Profile clicked');
  }
}
