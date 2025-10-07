import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteSelectedEvent, MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { Observable, startWith, map } from 'rxjs';
import { MatTooltipModule } from '@angular/material/tooltip';

interface AutocompleteOption {
  label: string;
  value: any;
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
export class AutocompleteComponent {
  @Input() label: string = '';
  @Input() placeholder: string = '';
  @Input() options: AutocompleteOption[] = [];
  @Input() control = new FormControl();
  @Input() cssClass?: string;
  @Input() prefixIcon?: string;
  @Input() suffixIcon?: string;
  @Input() clearable = false;
  @Input() clearTooltip = 'Limpar';
  @Input() hasError = false;
  @Input() errorMessage?: string;

  @Output() selectionChange = new EventEmitter<any>();
  @Output() suffixIconClick = new EventEmitter<void>();
  @Output() cleared = new EventEmitter<void>();

  filteredOptions: Observable<AutocompleteOption[]> = this.control.valueChanges.pipe(
    startWith(''),
    map(value => this._filter(value))
  );

  private _filter(value: string | any): AutocompleteOption[] {
    const filterValue = typeof value === 'string' ? value.toLowerCase() : '';
    return this.options.filter(option =>
      option.label.toLowerCase().includes(filterValue)
    );
  }

  displayFn = (option: any) => {
    const found = this.options.find(o => o.value === option);
    return found ? found.label : '';
  };

  onSelection(event: MatAutocompleteSelectedEvent) {
    this.selectionChange.emit(event.option.value);
  }

  onSuffixIconClick() {
    this.suffixIconClick.emit();
  }

  onClear() {
    this.control.setValue('');
    this.cleared.emit();
  }

  hasValue() {
    return !!this.control.value;
  }
}