import { Component, Input, Output, EventEmitter, forwardRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
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
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputComponent),
      multi: true,
    },
  ],
})
export class InputComponent implements ControlValueAccessor {
  @Input() label: string = '';
  @Input() placeholder: string = '';
  @Input() type: string = 'text';
  @Input() appearance: 'fill' | 'outline' = 'outline';
  @Input() required: boolean = false;
  @Input() disabled: boolean = false;
  @Input() readonly: boolean = false;
  @Input() clearable: boolean = false;
  @Input() clearTooltip: string = 'Limpar';
  @Input() cssClass: string = '';
  @Input() maxlength: number | null = null;
  @Input() minlength: number | null = null;
  @Input() pattern: string = '';
  @Input() autocomplete: string = 'off';
  @Input() hint: string = '';
  @Input() error: string = '';

  // Ícones
  @Input() prefixIcon: string = '';
  @Input() suffixIcon: string = '';
  @Input() prefixIconClass: string = 'prefix-icon';
  @Input() suffixIconClass: string = 'suffix-icon';

  @Output() inputChange = new EventEmitter<string>();
  @Output() clear = new EventEmitter<void>();
  @Output() suffixIconClick = new EventEmitter<void>();
  @Output() inputFocus = new EventEmitter<void>();
  @Output() inputBlur = new EventEmitter<void>();

  value: string = '';

  private onChange = (value: string) => {};
  private onTouched = () => {};

  writeValue(value: string): void {
    this.value = value || '';
  }

  registerOnChange(fn: (value: string) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  onInput(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.value = target.value;
    this.onChange(this.value);
    this.inputChange.emit(this.value);
  }

  onBlur(): void {
    this.onTouched();
    this.inputBlur.emit();
  }

  onFocus(): void {
    this.inputFocus.emit();
  }

  onClear(): void {
    this.value = '';
    this.onChange('');
    this.onTouched();
    this.clear.emit();
  }

  onSuffixIconClick(): void {
    this.suffixIconClick.emit();
  }

  hasValue(): boolean {
    return this.value !== '' && this.value !== null && this.value !== undefined;
  }
}
