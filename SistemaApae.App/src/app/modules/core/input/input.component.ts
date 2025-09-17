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
  template: `
    <mat-form-field [appearance]="appearance" [class]="cssClass">
      <mat-label *ngIf="label">{{ label }}</mat-label>

      <input
        matInput
        [type]="type"
        [value]="value"
        [placeholder]="placeholder"
        [required]="required"
        [disabled]="disabled"
        [readonly]="readonly"
        [attr.maxlength]="maxlength"
        [attr.minlength]="minlength"
        [pattern]="pattern"
        [autocomplete]="autocomplete"
        (input)="onInput($event)"
        (blur)="onBlur()"
        (focus)="onFocus()"
      />

      <mat-icon matPrefix *ngIf="prefixIcon" [class]="prefixIconClass">
        {{ prefixIcon }}
      </mat-icon>

      <mat-icon
        matSuffix
        *ngIf="suffixIcon && !clearable"
        [class]="suffixIconClass"
        (click)="onSuffixIconClick()"
      >
        {{ suffixIcon }}
      </mat-icon>

      <mat-icon
        matSuffix
        *ngIf="clearable && hasValue() && !disabled && !readonly"
        (click)="onClear()"
        class="clear-icon"
        [matTooltip]="clearTooltip"
      >
        clear
      </mat-icon>

      <mat-hint *ngIf="hint">{{ hint }}</mat-hint>
      <mat-error *ngIf="error">{{ error }}</mat-error>
    </mat-form-field>
  `,
  styles: [
    `
      mat-form-field {
        width: 100%;

        .clear-icon {
          cursor: pointer;
          color: var(--mat-on-surface-variant);
          font-size: 18px;
          width: 18px;
          height: 18px;
          transition: color 0.2s ease;

          &:hover {
            color: var(--mat-primary-500);
          }
        }

        .prefix-icon,
        .suffix-icon {
          color: var(--mat-on-surface-variant);
          cursor: pointer;

          &:hover {
            color: var(--mat-primary-500);
          }
        }

        &.field-xs {
          width: var(--field-width-xs);
          min-width: var(--field-width-xs);
        }

        &.field-sm {
          width: var(--field-width-sm);
          min-width: var(--field-width-sm);
        }

        &.field-md {
          width: var(--field-width-md);
          min-width: var(--field-width-md);
        }

        &.field-lg {
          width: var(--field-width-lg);
          min-width: var(--field-width-lg);
        }

        &.field-xl {
          width: var(--field-width-xl);
          min-width: var(--field-width-xl);
        }

        &.field-full {
          width: var(--field-width-full);
        }

        &.field-cpf {
          width: var(--field-width-cpf);
          min-width: var(--field-width-cpf);
        }

        &.field-cnpj {
          width: var(--field-width-cnpj);
          min-width: var(--field-width-cnpj);
        }

        &.field-cep {
          width: var(--field-width-cep);
          min-width: var(--field-width-cep);
        }

        &.field-phone {
          width: var(--field-width-phone);
          min-width: var(--field-width-phone);
        }

        &.field-date {
          width: var(--field-width-date);
          min-width: var(--field-width-date);
        }

        &.field-time {
          width: var(--field-width-time);
          min-width: var(--field-width-time);
        }

        &.field-number {
          width: var(--field-width-number);
          min-width: var(--field-width-number);
        }
      }
    `,
  ],
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
