import {
  Component,
  Input,
  Output,
  EventEmitter,
  OnInit,
  ChangeDetectionStrategy,
  signal,
  computed,
  inject
} from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormControl,
  ReactiveFormsModule,
  NG_VALUE_ACCESSOR,
  ControlValueAccessor,
  Form
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
      useExisting: AutocompleteMultipleComponent,
      multi: true
    }
  ]
})
export class AutocompleteMultipleComponent<T = any>
  implements OnInit, ControlValueAccessor
{
  readonly announcer = inject(LiveAnnouncer);
  readonly separatorKeysCodes: number[] = [ENTER, COMMA];

  @Input() label = '';
  @Input() placeholder = '';
  @Input() options: SelectOption[] = [];
  @Input() cssClass?: string ='example-chip-list';
  @Input() prefixIcon?: string;
  @Input() suffixIcon?: string;
  @Input() clearable = false;
  @Input() clearTooltip = 'Limpar';
  @Input() errorMessage?: string;
  @Input() control: FormControl<SelectOption[] | null> = new FormControl<SelectOption[]>([]);

  @Output() selectionChange = new EventEmitter<SelectOption[]>();
  @Output() suffixIconClick = new EventEmitter<void>();
  @Output() cleared = new EventEmitter<void>();

  onChange: any = () => {};
  onTouched: any = () => {};

  controlInput: FormControl<string | null> = new FormControl<string>('');

  filteredOptions!: Observable<SelectOption[]>

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

    return this.options.filter(option =>
      option.label.toLowerCase().includes(filterValue) && !(this.control?.value || []).some(selected => selected.value === option.value)
    );
  }


  writeValue(value: SelectOption[]): void {
    this.control.setValue(value || []);
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  add(event: MatChipInputEvent): void {
    const value = (event.value || '').trim();
    if (!value) return;

    const match = this.options.find(
      o => o.label.toLowerCase() === value.toLowerCase()
    );
    if (match) {
      this.control.setValue([...this.control.value as SelectOption[], match]);
      this._updateValue();
   
    }

    this.controlInput.patchValue('');
  }

  remove(option: SelectOption): void {
    const list = this.control.value as SelectOption[];

    const index = list.findIndex(o => o.value === option.value);
    if (index >= 0) list.splice(index, 1);
    this.announcer.announce(`Removido ${option.label}`);
    this.control.setValue([...list]);

    this._updateValue();
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    const option = event.option.value as SelectOption;
    this.control.setValue([...this.control.value || [], option]);
    this.controlInput.setValue('');
    this._updateValue();
    event.option.deselect();
  }

  clearAll(): void {
    this.control.setValue([]);
    this.controlInput.setValue('');
    this._updateValue();
    this.cleared.emit();
  }

  hasValue(): boolean {
    return (this.control.value || []).length > 0;
  }

  onSuffixIconClick(): void {
    this.suffixIconClick.emit();
  }

  private _updateValue(): void {
    const value = this.control.value;
    this.onChange(value);
    this.selectionChange.emit(value || []);
  }
}
