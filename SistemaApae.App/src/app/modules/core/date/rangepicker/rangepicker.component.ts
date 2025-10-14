import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-rangepicker',
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
  templateUrl: './rangepicker.component.html',
  styleUrls: ['./rangepicker.component.less'],
})
export class RangePickerComponent {
  /** Rótulo principal do campo */
  @Input() label: string = 'Período';

  /** Aparência do campo */
  @Input() appearance: 'fill' | 'outline' = 'outline';

  /** Controles reativos */
  @Input() controlInicio!: FormControl<Date | null>;
  @Input() controlFim!: FormControl<Date | null>;

  /** Limites e configurações */
  @Input() minDate: Date | null = null;
  @Input() maxDate: Date | null = null;
  @Input() touchUi: boolean = false;

  /** Ícones e comportamento */
  @Input() clearable: boolean = true;
  @Input() clearTooltip: string = 'Limpar intervalo';
  @Input() cssClass: string = '';

  /** Eventos externos */
  @Output() rangeChange = new EventEmitter<{ inicio: Date | null; fim: Date | null }>();
  @Output() clear = new EventEmitter<void>();

  onDateChange(): void {
    this.rangeChange.emit({
      inicio: this.controlInicio?.value ?? null,
      fim: this.controlFim?.value ?? null,
    });
  }

  onClear(): void {
    this.controlInicio?.setValue(null);
    this.controlFim?.setValue(null);
    this.clear.emit();
    this.onDateChange();
  }

  get hasValue(): boolean {
    return !!(this.controlInicio?.value || this.controlFim?.value);
  }
}
