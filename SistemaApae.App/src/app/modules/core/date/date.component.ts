import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormControl } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-date',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatIconModule,
    MatTooltipModule,
  ],
  templateUrl: './date.component.html',
  styleUrls: ['./date.component.less'],
})
export class DateComponent {
  @Input() label: string = '';
  @Input() placeholder: string = 'DD/MM/AAAA';
  @Input() appearance: 'fill' | 'outline' = 'outline';
  @Input() clearable: boolean = false;
  @Input() clearTooltip: string = 'Limpar';
  @Input() cssClass: string = '';
  @Input() control!: FormControl;

  // Configurações do datepicker
  @Input() startDate: Date | null = null;
  @Input() maxDate: Date | null = null;
  @Input() minDate: Date | null = null;
  @Input() touchUi: boolean = false;

  // Ícones
  @Input() prefixIcon: string = '';

  @Output() dateChange = new EventEmitter<Date | null>();
  @Output() clear = new EventEmitter<void>();
  @Output() dateInput = new EventEmitter<Date | null>();
  @Output() dateFocus = new EventEmitter<void>();
  @Output() dateBlur = new EventEmitter<void>();

  onDateChange(date: Date | null): void {
    this.dateChange.emit(date);
  }

  onDateInput(date: Date | null): void {
    this.dateInput.emit(date);
  }

  onBlur(): void {
    this.dateBlur.emit();
  }

  onFocus(): void {
    this.dateFocus.emit();
  }

  onClear(): void {
    this.control.setValue(null);
    this.clear.emit();
  }

  hasValue(): boolean {
    const value = this.control.value;
    return value !== '' && value !== null && value !== undefined;
  }

  get hasError(): boolean {
    return this.control ? !!(this.control.invalid && this.control.touched) : false;
  }

  get errorMessage(): string {
    if (!this.control?.errors) return '';

    const errors = this.control.errors;
    if (errors['required']) return `${this.label || 'Data'} é obrigatória`;
    if (errors['matDatepickerParse']) return 'Data inválida';
    if (errors['matDatepickerMin']) return 'Data anterior ao permitido';
    if (errors['matDatepickerMax']) return 'Data posterior ao permitido';

    return Object.keys(errors)[0] || '';
  }
}
