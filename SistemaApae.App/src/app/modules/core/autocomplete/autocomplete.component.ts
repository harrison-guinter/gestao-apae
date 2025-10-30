import { Component, Input, Output, EventEmitter, ViewChild, OnInit } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import {
  MatAutocompleteSelectedEvent,
  MatAutocompleteModule,
} from '@angular/material/autocomplete';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInput, MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
<<<<<<< HEAD
import { Observable, startWith, map, debounceTime } from 'rxjs';
=======
import { Observable, startWith, map, debounce, debounceTime } from 'rxjs';
>>>>>>> 632c473c0dcfcb36dde57393f1117fe75d5fe91b
import { MatTooltipModule } from '@angular/material/tooltip';
import { SelectOption } from '../select/select.component';

@Component({
  selector: 'app-autocomplete',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatAutocompleteModule,
    MatTooltipModule,
    MatIconModule,
  ],
  templateUrl: './autocomplete.component.html',
  styleUrls: ['./autocomplete.component.less'],
})
export class AutocompleteComponent<T = any> implements OnInit {
  @ViewChild('input') input?: MatInput;

  @Input() label = '';
  @Input() placeholder = '';
  @Input() options: SelectOption[] = [];
  @Input() control = new FormControl<SelectOption | null>(null);
  @Input() cssClass?: string;
  @Input() prefixIcon?: string;
  @Input() suffixIcon?: string;
  @Input() clearable = false;
  @Input() clearTooltip = 'Limpar';
  @Input() hasError = false;
  @Input() errorMessage?: string;

  @Output() selectionChange = new EventEmitter<SelectOption>();
  @Output() suffixIconClick = new EventEmitter<void>();
  @Output() cleared = new EventEmitter<void>();

  filteredOptions!: Observable<SelectOption[]>;

  ngOnInit() {
    this.filteredOptions = this.control.valueChanges.pipe(
      debounceTime(200),
      startWith(''),
      map((value) => this._filter(value))
    );
  }

  private _filter(value: SelectOption | string | null): SelectOption[] {
    const filterValue =
      typeof value === 'string' ? value.toLowerCase() : value?.label?.toLowerCase() || '';

    return this.options.filter(option => 
      option.label.toLowerCase().includes(filterValue)
    );
  }

  displayFn(option: SelectOption | null): string {
    return option ? option.label : '';
  }

  onSelection(event: MatAutocompleteSelectedEvent) {
    const selectedOption = event.option.value as SelectOption;
    this.control.setValue(selectedOption);
    this.selectionChange.emit(selectedOption);
  }

  onSuffixIconClick() {
    this.suffixIconClick.emit();
  }

  onClear() {
    this.control.setValue(null);
    this.cleared.emit();
  }

  onBlur() {
    if (this.control.value && typeof this.control.value === 'string') {
      this.control.setValue(null);
    }
  }

  hasValue() {
    return !!this.control.value;
  }

  static selectOptionValidator(control: any) {
    const value = control.value;
    if (!value) return null;
    if (typeof value === 'object' && 'value' in value && 'label' in value) return null;
    return { invalidSelectOption: true };
  }
}
