import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { Subscription } from 'rxjs';
import { PageInfoService, PageInfo } from '../services/page-info.service';
import { AuthService } from '../../auth/auth.service';
import { NotificationService } from '../notification/notification.service';
import { Roles } from '../../auth/roles.enum';
import { V } from '@angular/cdk/keycodes';
import { ConfirmationService } from '../confirmation-modal/confirmation.service';

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

  @ViewChild('logoutIcon') logoutIcon!: MatIcon;

  private pageInfoSubscription?: Subscription;

  constructor(
    private router: Router,
    private pageInfoService: PageInfoService,
    private authService: AuthService,
    private notificationService: NotificationService,
    private confirmationService: ConfirmationService
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

        this.userName = user.nome || user.name || 'Usuário';
        this.userRole = user.perfil || 'Perfil';

        this.generateUserInitials();
      }
    } catch (error) {
      this.userName = 'Usuário';
      this.userRole = 'Perfil';
      this.userInitials = 'U';
    }
  }

  private generateUserInitials(): void {
    const names = this.userName
      .trim()
      .split(' ')
      .filter((name) => name.length > 0);

    if (names.length >= 2) {
      this.userInitials = names[0][0] + names[names.length - 1][0];
    } else if (names.length === 1) {
      const name = names[0];
      this.userInitials = name.length > 1 ? name[0] + name[1] : name[0] + name[0];
    } else {
      this.userInitials = 'U';
    }

    this.userInitials = this.userInitials.toUpperCase();
  }

  ngOnDestroy() {
    if (this.pageInfoSubscription) {
      this.pageInfoSubscription.unsubscribe();
    }
  }

  logout(): void {
    const config = {
      message: 'Tem certeza que deseja sair do sistema?',
      confirmButtonText: 'Sim',
      cancelButtonText: 'Não',
      elementRef: this.logoutIcon._elementRef.nativeElement,
      disableClose: true,
    };

    this.confirmationService.openConfirmationModal(config).subscribe((confirmed) => {
      if (confirmed) {
        try {
          this.authService.logout();
          this.notificationService.success('Logout realizado com sucesso!');
          this.router.navigate(['/login']);
        } catch (error) {
          console.error('Erro durante logout:', error);
          this.notificationService.fail('Erro ao realizar logout');
        }
      }
    });
  }
}
