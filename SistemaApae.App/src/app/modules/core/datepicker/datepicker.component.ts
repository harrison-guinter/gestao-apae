import { Component, Input, forwardRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, ReactiveFormsModule, NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { MatFormFieldAppearance, MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerInputEvent, MatDatepickerModule } from '@angular/material/datepicker';

@Component({
  selector: 'app-datepicker',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatIconModule,
  ],
  template: `
    <mat-form-field [class]="cssClass" class="full-width ">
      <mat-label>{{ label }}</mat-label>
      <input
        matInput
        [matDatepicker]="picker"
        [placeholder]="placeholder"
        [formControl]="control"
        (dateChange)="onDateChange($event)"
        (input)="onDateInput($event)"
      />
      <mat-datepicker-toggle matSuffix [for]="picker"></mat-datepicker-toggle>
      <mat-datepicker #picker></mat-datepicker>
    </mat-form-field>
  `,
  styles: [`
    .full-width { width: 100%; }
  `],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DatepickerComponent),
      multi: true
    }
  ]
})
export class DatepickerComponent implements ControlValueAccessor {
  @Input() label: string = '';
  @Input() placeholder: string = '';
  @Input() cssClass: string = '';

  control = new FormControl();

  private onChangeFn: (value: Date | null) => void = () => {};
  private onTouchedFn: () => void = () => {};

  writeValue(value: Date | null): void {
    this.control.setValue(value);
  }

  registerOnChange(fn: any): void {
    this.onChangeFn = fn;
    this.control.valueChanges.subscribe(fn);
  }

  registerOnTouched(fn: any): void {
    this.onTouchedFn = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    if (isDisabled) {
      this.control.disable();
    } else {
      this.control.enable();
    }
  }

  // Reset automático quando valor digitado for inválido
  onDateChange(event: MatDatepickerInputEvent<Date>) {
    if (!(event.value instanceof Date) || isNaN(event.value.getTime())) {
      this.control.reset();
    }
  }

  onDateInput(event: any) {
    const inputValue = event.target.value;
    const parsed = new Date(inputValue);
    if (inputValue && (isNaN(parsed.getTime()) || inputValue.length >= 10)) {
      this.control.reset();
    }
  }
}
