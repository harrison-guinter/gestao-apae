import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormControl } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-input',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatTooltipModule,
  ],
  templateUrl: './input.component.html',
  styleUrls: ['./input.component.less'],
})
export class InputComponent {
  @Input() label: string = '';
  @Input() placeholder: string = '';
  @Input() type: string = 'text';
  @Input() appearance: 'fill' | 'outline' = 'outline';
  @Input() clearable: boolean = false;
  @Input() clearTooltip: string = 'Limpar';
  @Input() cssClass: string = '';
  @Input() maxlength: number | null = null;
  @Input() minlength: number | null = null;
  @Input() pattern: string = '';
  @Input() autocomplete: string = 'off';
  @Input() control!: FormControl;

  // Ícones
  @Input() prefixIcon: string = '';
  @Input() suffixIcon: string = '';

  @Output() inputChange = new EventEmitter<string>();
  @Output() clear = new EventEmitter<void>();
  @Output() suffixIconClick = new EventEmitter<void>();
  @Output() inputFocus = new EventEmitter<void>();
  @Output() inputBlur = new EventEmitter<void>();

  onInput(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.inputChange.emit(target.value);
  }

  onBlur(): void {
    this.inputBlur.emit();
  }

  onFocus(): void {
    this.inputFocus.emit();
  }

  onClear(): void {
    this.control.setValue('');
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
