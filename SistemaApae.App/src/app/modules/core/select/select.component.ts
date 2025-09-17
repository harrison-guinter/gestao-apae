import { Component, Input, Output, EventEmitter, forwardRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
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
  template: `
    <mat-form-field [appearance]="appearance" [class]="cssClass">
      <mat-label *ngIf="label">{{ label }}</mat-label>

      <mat-select
        [value]="value"
        [placeholder]="placeholder"
        [required]="required"
        [disabled]="disabled"
        [multiple]="multiple"
        (selectionChange)="onSelectionChange($event.value)"
      >
        <mat-option
          *ngFor="let option of options"
          [value]="option.value"
          [disabled]="option.disabled"
        >
          {{ option.label }}
        </mat-option>
      </mat-select>

      <mat-icon
        matSuffix
        *ngIf="clearable && hasValue() && !disabled"
        (click)="onClear()"
        class="clear-icon"
        [matTooltip]="clearTooltip"
      >
        clear
      </mat-icon>
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
      }
    `,
  ],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SelectComponent),
      multi: true,
    },
  ],
})
export class SelectComponent implements ControlValueAccessor {
  @Input() label: string = '';
  @Input() placeholder: string = '';
  @Input() options: SelectOption[] = [];
  @Input() appearance: 'fill' | 'outline' = 'outline';
  @Input() required: boolean = false;
  @Input() disabled: boolean = false;
  @Input() multiple: boolean = false;
  @Input() clearable: boolean = true;
  @Input() clearTooltip: string = 'Limpar';
  @Input() cssClass: string = '';

  @Output() selectionChange = new EventEmitter<any>();
  @Output() clear = new EventEmitter<void>();

  value: any = '';

  private onChange = (value: any) => {};
  private onTouched = () => {};

  writeValue(value: any): void {
    this.value = value || '';
  }

  registerOnChange(fn: (value: any) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  onSelectionChange(value: any): void {
    this.value = value;
    this.onChange(value);
    this.onTouched();
    this.selectionChange.emit(value);
  }

  onClear(): void {
    this.value = '';
    this.onChange('');
    this.onTouched();
    this.clear.emit();
  }

  hasValue(): boolean {
    return this.value !== '' && this.value !== null && this.value !== undefined;
  }
}
