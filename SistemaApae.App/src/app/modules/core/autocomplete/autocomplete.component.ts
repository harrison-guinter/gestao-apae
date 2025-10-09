import { Component, Input, Output, EventEmitter, ViewChild, OnInit } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteSelectedEvent, MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInput, MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { Observable, startWith, map } from 'rxjs';
import { MatTooltipModule } from '@angular/material/tooltip';

export interface SelectOption<T = any> {
  label: string;
  value: T;
  disabled?: boolean;
}

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
    MatIconModule
  ],
  templateUrl: './autocomplete.component.html',
  styleUrls: ['./autocomplete.component.less']
})
export class AutocompleteComponent<T = any> implements OnInit {
  @ViewChild('input') input?: MatInput;

  @Input() label = '';
  @Input() placeholder = '';
  @Input() options: SelectOption<T>[] = [];
  @Input() control = new FormControl<SelectOption<T> | null>(null);
  @Input() cssClass?: string;
  @Input() prefixIcon?: string;
  @Input() suffixIcon?: string;
  @Input() clearable = false;
  @Input() clearTooltip = 'Limpar';
  @Input() hasError = false;
  @Input() errorMessage?: string;

  @Output() selectionChange = new EventEmitter<SelectOption<T>>();
  @Output() suffixIconClick = new EventEmitter<void>();
  @Output() cleared = new EventEmitter<void>();

  filteredOptions!: Observable<SelectOption<T>[]>;

  ngOnInit() {
    this.filteredOptions = this.control.valueChanges.pipe(
      startWith(''),
      map(value => this._filter(value))
    );
  }

  private _filter(value: SelectOption<T> | string | null): SelectOption<T>[] {
    const filterValue =
      typeof value === 'string'
        ? value.toLowerCase()
        : value?.label?.toLowerCase() || '';

    return this.options.filter(option =>
      option.label.toLowerCase().includes(filterValue)
    );
  }

  displayFn(option: SelectOption<T> | null): string {
    return option ? option.label : '';
  }

  onSelection(event: MatAutocompleteSelectedEvent) {
    const selectedOption = event.option.value as SelectOption<T>;
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

  hasValue() {
    return !!this.control.value;
  }
}
