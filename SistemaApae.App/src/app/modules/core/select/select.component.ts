import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormControl } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';

export interface SelectOption {
  value: any;
  label: string;
  disabled?: boolean;
}

@Component({
  selector: 'app-select',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatIconModule,
    MatTooltipModule,
  ],
  templateUrl: './select.component.html',
  styleUrls: ['./select.component.less'],
})
export class SelectComponent {
  @Input() label: string = '';
  @Input() placeholder: string = '';
  @Input() options: SelectOption[] = [];
  @Input() appearance: 'fill' | 'outline' = 'outline';
  @Input() clearable: boolean = true;
  @Input() clearTooltip: string = 'Limpar';
  @Input() cssClass: string = '';
  @Input() prefixIcon: string = '';
  @Input() suffixIcon: string = '';
  @Input() control!: FormControl;

  @Output() selectionChange = new EventEmitter<any>();
  @Output() clear = new EventEmitter<void>();
  @Output() suffixIconClick = new EventEmitter<void>();

  compareWith = (o1: any, o2: any): boolean => {
    console.log('Comparing:', o1, o2);
    // Se são objetos SelectOption
    // console.log(o1 && o2 && typeof o1 === 'object' && typeof o2 === 'object');
    if (o1 && o2 && typeof o1 === 'object' && typeof o2 === 'object') {

      return o1.value === o2.value;
    }

    // console.log(o1 && typeof o1 === 'object' && o1.value !== undefined)
    // Se o1 é um SelectOption e o2 é um valor primitivo
    if (o1 && typeof o1 === 'object' && o1.value !== undefined) {
      return o1.value === o2;
    }

    // console.log(o2 && typeof o2 === 'object' && o2.value !== undefined)
    // Se o2 é um SelectOption e o1 é um valor primitivo
    if (o2 && typeof o2 === 'object' && o2.value !== undefined) {
      return o1 === o2.value;
    }

   
    // Comparação direta para valores primitivos
    return o1 === o2;
  };

  onSelectionChange(value: any): void {
    this.selectionChange.emit(value);
  }

  onClear(): void {
    this.control.setValue(null);
    this.clear.emit();
  }

  onSuffixIconClick(): void {
    this.suffixIconClick.emit();
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
    if (errors['required']) return `${this.label || 'Campo'} é obrigatório`;
    if (errors['minlength']) return `Mínimo ${errors['minlength'].requiredLength} caracteres`;
    if (errors['maxlength']) return `Máximo ${errors['maxlength'].requiredLength} caracteres`;
    if (errors['email']) return 'E-mail inválido';
    if (errors['pattern']) return 'Formato inválido';

    return Object.keys(errors)[0] || '';
  }
}
