import {
  Component,
  Input,
  Output,
  EventEmitter,
  ViewChild,
  OnInit
} from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormControl,
  ReactiveFormsModule,
  NG_VALUE_ACCESSOR,
  ControlValueAccessor
} from '@angular/forms';
import { MatAutocompleteSelectedEvent, MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInput, MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import { Observable, map, startWith } from 'rxjs';

export interface SelectOption<T = any> {
  label: string;
  value: T;
  disabled?: boolean;
}

@Component({
  selector: 'app-autocomplete-multiple',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatAutocompleteModule,
    MatTooltipModule,
    MatIconModule,
    MatChipsModule
  ],
  templateUrl: './multi-autocomplete.component.html',
  styleUrls: ['./multi-autocomplete.component.less'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: AutocompleteMultipleComponent,
      multi: true
    }
  ]
})
export class AutocompleteMultipleComponent
  implements OnInit, ControlValueAccessor
{
  @ViewChild('input') input?: MatInput;

  @Input() label = '';
  @Input() placeholder = '';
  @Input() options: SelectOption[] = [];
  @Input() cssClass?: string;
  @Input() prefixIcon?: string;
  @Input() suffixIcon?: string;
  @Input() clearable = false;
  @Input() clearTooltip = 'Limpar';
  @Input() errorMessage?: string;

  @Output() selectionChange = new EventEmitter<SelectOption[]>();
  @Output() suffixIconClick = new EventEmitter<void>();
  @Output() cleared = new EventEmitter<void>();

  control = new FormControl('');
  selectedOptions: SelectOption[] = [];
  filteredOptions!: Observable<SelectOption[]>;

  onChange: any = () => {};
  onTouched: any = () => {};

  ngOnInit() {
    this.filteredOptions = this.control.valueChanges.pipe(
      startWith(''),
      map(value => this._filter(value || ''))
    );
  }

  writeValue(value: SelectOption[]): void {
    this.selectedOptions = value || [];
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  private _filter(value: string): SelectOption[] {
    const filterValue = value.toLowerCase();
    return this.options.filter(
      option =>
        option.label.toLowerCase().includes(filterValue) &&
        !this.selectedOptions.some(sel => sel.value === option.value)
    );
  }

  onSelection(event: MatAutocompleteSelectedEvent) {
    const selected = event.option.value as SelectOption;
    this.selectedOptions.push(selected);
    this._updateValue();
    this.control.setValue('');
  }

  remove(option: SelectOption) {
    const index = this.selectedOptions.findIndex(o => o.value === option.value);
    if (index >= 0) {
      this.selectedOptions.splice(index, 1);
      this._updateValue();
    }
  }

  clearAll() {
    this.selectedOptions = [];
    this.control.setValue('');
    this._updateValue();
    this.cleared.emit();
  }

  private _updateValue() {
    this.onChange(this.selectedOptions);
    this.selectionChange.emit(this.selectedOptions);
  }

  hasValue() {
    return this.selectedOptions.length > 0;
  }

  onSuffixIconClick() {
    this.suffixIconClick.emit();
  }
}
