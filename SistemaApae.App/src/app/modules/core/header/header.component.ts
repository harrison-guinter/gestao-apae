import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { Subscription } from 'rxjs';
import { PageInfoService, PageInfo } from '../services/page-info.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.less'],
})
export class HeaderComponent implements OnInit, OnDestroy {
  userName: string = 'Luana Marini';
  userRole: string = 'Coordenador';
  userInitials: string = 'LM';
  pageTitle: string = 'Dashboard';
  pageSubtitle: string = 'Sistema de Gestão de Atendimentos';

  private pageInfoSubscription?: Subscription;

  constructor(private router: Router, private pageInfoService: PageInfoService) {}

  ngOnInit() {
    this.pageInfoSubscription = this.pageInfoService.pageInfo$.subscribe((pageInfo: PageInfo) => {
      this.pageTitle = pageInfo.title;
      this.pageSubtitle = pageInfo.subtitle;
    });
  }

  ngOnDestroy() {
    if (this.pageInfoSubscription) {
      this.pageInfoSubscription.unsubscribe();
    }
  }

  logout(): void {
    console.log('Logout clicked');
    // this.router.navigate(['/login']);
  }

  onProfileClick(): void {
    console.log('Profile clicked');
  }
}
