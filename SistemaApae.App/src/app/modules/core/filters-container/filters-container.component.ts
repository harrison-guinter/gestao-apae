import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-filters-container',
  standalone: true,
  templateUrl: './filters-container.component.html',
  styleUrls: ['./filters-container.component.less'],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatTooltipModule,
  ],
})
export class FiltersContainerComponent {
  @Input() formGroup!: FormGroup;
  @Input() disabled: boolean = false;
  @Input() showAddButton: boolean = true;
  @Input() clearTooltip: string = 'Limpar filtros';
  @Input() addTooltip: string = 'Novo';
  @Input() title: string = 'Lista';
  @Input() isRelatorio: boolean = false;

  @Output() clear = new EventEmitter<void>();
  @Output() add = new EventEmitter<void>();
  @Output() search = new EventEmitter<void>();

  onClear(): void {
    this.clear.emit();
  }

  onAdd(): void {
    this.add.emit();
  }

  onSearch(): void {
    this.search.emit();
  }
}
