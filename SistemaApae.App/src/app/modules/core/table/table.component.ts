import { Component, Input, Output, EventEmitter, TemplateRef, ContentChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

export interface TableColumn {
  key: string;
  label: string;
  sortable?: boolean;
  template?: TemplateRef<any>;
  width?: 'small' | 'medium' | 'large' | 'xlarge' | 'auto';
  align?: 'left' | 'center' | 'right';
  getClass?: (row: any) => string;
  getCellValue?: (row: any) => any;
  type?: 'date' | 'datetime' | 'text';
}

export interface TableAction {
  icon: string;
  tooltip: string;
  color?: 'primary' | 'accent' | 'warn';
  action: (row: any) => void;
}

@Component({
  selector: 'app-table',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule, MatIconModule],
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.less'],
})
export class TableComponent {
  @Input() columns: TableColumn[] = [];
  @Input() data: any[] = [];
  @Input() actions: TableAction[] = [];
  @Input() showActions: boolean = true;
  @Input() tableClass: string = '';
  @Input() showFooter: boolean = true;

  @Output() rowClick = new EventEmitter<any>();
  @Output() actionClick = new EventEmitter<{ action: string; row: any }>();

  @ContentChild('cellTemplate') cellTemplate!: TemplateRef<any>;

  get displayedColumns(): string[] {
    const cols = this.columns.map((col) => col.key);
    if (this.showActions && this.actions.length > 0) {
      cols.push('actions');
    }
    return cols;
  }

  onRowClick(row: any) {
    this.rowClick.emit(row);
  }

  onActionClick(action: TableAction, row: any, event: Event) {
    event.stopPropagation();
    action.action(row);
    this.actionClick.emit({ action: action.tooltip, row });
  }

  getCellValue(row: any, column: TableColumn): any {
    const value = this.getNestedProperty(row, column.key);

    if (column.type === 'date' && value) {
      return this.formatDate(value, false);
    }

    if (column.type === 'datetime' && value) {
      return this.formatDate(value, true);
    }

    return value;
  }

  private formatDate(value: any, includeTime: boolean = false): string {
    try {
      const date = new Date(value);

      if (isNaN(date.getTime())) {
        return value;
      }

      const day = String(date.getDate()).padStart(2, '0');
      const month = String(date.getMonth() + 1).padStart(2, '0');
      const year = date.getFullYear();

      const dateStr = `${day}/${month}/${year}`;

      if (includeTime) {
        const hours = String(date.getHours()).padStart(2, '0');
        const minutes = String(date.getMinutes()).padStart(2, '0');
        const seconds = String(date.getSeconds()).padStart(2, '0');
        return `${dateStr} ${hours}:${minutes}:${seconds}`;
      }

      return dateStr;
    } catch {
      return value;
    }
  }

  getColumnClass(column: TableColumn): string {
    const classes = [`mat-column-${column.key}`];

    if (column.width) {
      classes.push(`column-${column.width}`);
    }

    if (column.align) {
      classes.push(`column-${column.align}`);
    }

    return classes.join(' ');
  }

  hasCustomTemplate(columnKey: string): boolean {
    return ['tipo', 'status'].includes(columnKey);
  }

  private getNestedProperty(obj: any, path: string): any {
    return path.split('.').reduce((o, p) => o && o[p], obj);
  }

  get totalRecords(): number {
    return this.data?.length || 0;
  }
}
