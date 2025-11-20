import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import {
  DateAdapter,
  MatNativeDateModule,
  MAT_DATE_FORMATS,
  NativeDateAdapter,
} from '@angular/material/core';
import { PageInfoService } from '../core/services/page-info.service';
import { DashboardData, DashboardService } from './dashboard.service';

export class MonthYearDateAdapter extends NativeDateAdapter {
  override format(date: Date, displayFormat: Object): string {
    if (displayFormat === 'input') {
      const month = date.getMonth() + 1;
      const year = date.getFullYear();
      return `${month.toString().padStart(2, '0')}/${year}`;
    }
    return date.toDateString();
  }
}

export const MONTH_YEAR_FORMATS = {
  parse: {
    dateInput: 'MM/YYYY',
  },
  display: {
    dateInput: 'input',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
  ],
  providers: [
    {
      provide: DateAdapter,
      useClass: MonthYearDateAdapter,
    },
    {
      provide: MAT_DATE_FORMATS,
      useValue: MONTH_YEAR_FORMATS,
    },
  ],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.less'],
})
export class DashboardComponent implements OnInit {
  dashboardData: DashboardData | null = null;
  protected isProfissional: boolean;
  protected filtrosForm!: UntypedFormGroup;

  constructor(
    private pageInfoService: PageInfoService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private formBuilder: UntypedFormBuilder,
    private dashboardService: DashboardService
  ) {
    this.isProfissional = this.getIsProfissional();
  }

  getIsProfissional(): boolean {
    return JSON.parse(localStorage.getItem('usuario')!).perfil === 'Profissional';
  }

  ngOnInit() {
    this.pageInfoService.updatePageInfo('Dashboard', 'Sistema de Gestão de Atendimentos');
    this.initFiltrosForm();
    this.carregarDados();
  }

  initFiltrosForm() {
    const hoje = new Date();
    this.filtrosForm = this.formBuilder.group({
      periodoReferencia: [hoje],
    });
  }

  carregarDados() {
    const periodoReferencia = this.filtrosForm.get('periodoReferencia')?.value;
    const year = periodoReferencia?.getFullYear();
    const month = periodoReferencia ? periodoReferencia.getMonth() + 1 : undefined;

    this.dashboardService
      .buscarDadosDashboard(year, month)
      .subscribe((data: any) => (this.dashboardData = data.data));
  }

  onMonthYearChange() {
    this.carregarDados();
  }

  chosenYearHandler(normalizedYear: Date) {
    const ctrlValue = this.filtrosForm.get('periodoReferencia')?.value || new Date();
    ctrlValue.setFullYear(normalizedYear.getFullYear());
    this.filtrosForm.get('periodoReferencia')?.setValue(ctrlValue);
  }

  chosenMonthHandler(normalizedMonth: Date, datepicker: any) {
    const ctrlValue = this.filtrosForm.get('periodoReferencia')?.value || new Date();
    ctrlValue.setMonth(normalizedMonth.getMonth());
    ctrlValue.setFullYear(normalizedMonth.getFullYear());
    this.filtrosForm.get('periodoReferencia')?.setValue(ctrlValue);
    datepicker.close();
    this.onMonthYearChange();
  }

  navigateToAssistidos(): void {
    this.router.navigate(['/home/assistidos/cadastro']);
  }

  navigateToUsuarios(): void {
    this.router.navigate(['/home/usuarios'], { queryParams: { isNew: 'true' } });
  }

  navigateToAgendamentos(): void {
    this.router.navigate(['/home/agendamentos']);
  }

  navigateToAtendimentos(): void {
    if (this.isProfissional) {
      this.router.navigate(['/home/atendimentos-pendentes']);
      return;
    }
    this.router.navigate(['/home/atendimentos-realizados']);
  }
}
