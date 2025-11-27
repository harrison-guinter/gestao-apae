import {
  Component,
  Input,
  Output,
  EventEmitter,
  OnInit,
  ChangeDetectionStrategy,
  forwardRef,
  inject
} from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormControl,
  ReactiveFormsModule,
  NG_VALUE_ACCESSOR,
  ControlValueAccessor,
  Validators,
  FormGroup
} from '@angular/forms';
import {
  MatAutocompleteModule,
  MatAutocompleteSelectedEvent
} from '@angular/material/autocomplete';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatChipsModule, MatChipInputEvent } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { LiveAnnouncer } from '@angular/cdk/a11y';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { SelectOption } from '../select/select.component';
import { map, Observable, startWith } from 'rxjs';

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
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => AutocompleteMultipleComponent),
      multi: true
    }
  ]
})
export class AutocompleteMultipleComponent<T = any>
  implements OnInit, ControlValueAccessor {
  readonly announcer = inject(LiveAnnouncer);
  readonly separatorKeysCodes: number[] = [ENTER, COMMA];

  @Input() label = '';
  @Input() placeholder = '';
  @Input() options: SelectOption[] = [];
  @Input() cssClass?: string = 'example-chip-list';
  @Input() prefixIcon?: string;
  @Input() suffixIcon?: string;
  @Input() clearable = false;
  @Input() clearTooltip = 'Limpar';
  @Input() errorMessage?: string;

  /** Controle usado para validação */
  @Input() control?: FormControl<SelectOption[] | null>;

  @Output() selectionChange = new EventEmitter<SelectOption[] | null>();
  @Output() suffixIconClick = new EventEmitter<void>();
  @Output() cleared = new EventEmitter<void>();

  onChange: any = () => { };
  onTouched: any = () => {};

  controlInput = new FormControl<string | null>('');
  filteredOptions!: Observable<SelectOption[]>;

  ngOnInit() {
    this.filteredOptions = this.controlInput.valueChanges.pipe(
      startWith(''),
      map(value => this._filter(value))
    );
  }


  private _filter(value: SelectOption | string | null): SelectOption[] {
    const filterValue =
      typeof value === 'string'
        ? value.toLowerCase()
        : value?.label?.toLowerCase() || '';

    return this.options.filter(
      option =>
        option.label.toLowerCase().includes(filterValue) &&
        !(this.control?.value || []).some(
          selected => selected.value === option.value
        )
    );
  }

  writeValue(value: SelectOption[]): void {
    this.control?.setValue(value);
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    if (isDisabled) {
      this.control?.disable();
      this.controlInput.disable();
    } else {
      this.control?.enable();
      this.controlInput.enable();
    }
  }

  add(event: MatChipInputEvent): void {
    const value = (event.value || '').trim();
    if (!value) return;

    const match = this.options.find(
      o => o.label.toLowerCase() === value.toLowerCase()
    );
    if (match) {
      this.control?.setValue([
        ...(this.control.value || []),
        match
      ]);
      this._updateValue();
    }

    this.controlInput.patchValue('');
  }

  remove(option: SelectOption): void {
    const list = this.control?.value as SelectOption[];
    const index = list.findIndex(o => o.value === option.value);
    if (index >= 0) list.splice(index, 1);
    this.announcer.announce(`Removido ${option.label}`);
    this.control?.setValue([...list]);
  
    this._updateValue();
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    const option = event.option.value as SelectOption;
    this.control?.setValue([...(this.control?.value || []), option]);
    this.controlInput.setValue('');
    this._updateValue();
    event.option.deselect();
  }

  clearAll(): void {
    this.control?.setValue(null);
    this.controlInput.setValue('');
    this._updateValue();
    this.cleared.emit();
  }

  hasValue(): boolean {
    return this.control?.value ? this.control.value.length > 0 : false;
  }

  onSuffixIconClick(): void {
    this.suffixIconClick.emit();
  }

  private _updateValue(): void {
    const value = this.control?.value;
    this.onChange(value);
    this.selectionChange.emit(value || null);
    this.control?.updateValueAndValidity();
  }
}
