import { Injectable, Type } from '@angular/core';
import { MatDialog, MatDialogRef, MatDialogConfig } from '@angular/material/dialog';
import { Observable } from 'rxjs';

export interface ModalConfig {
  component: Type<any>;
  width?: string;
  height?: string;
  maxWidth?: string;
  maxHeight?: string;
  element?: any;
  data?: any;
  isVisualizacao?: boolean;
  disableClose?: boolean;
}

export interface ModalData {
  element?: any;
  data?: any;
  isVisualizacao?: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class ModalService {
  constructor(private dialog: MatDialog) {}

  openModal<T = any, R = any>(config: ModalConfig): Observable<R | undefined> {
    const dialogConfig: MatDialogConfig = {
      width: config.width || '50%',
      height: config.height || 'auto',
      maxWidth: config.maxWidth || '90vw',
      maxHeight: config.maxHeight || '90vh',
      disableClose: config.disableClose || false,
      data: {
        isVisualizacao: config.isVisualizacao || false,
        element: config.element,
        data: config.data,
      } as ModalData,
    };

    const dialogRef: MatDialogRef<T, R> = this.dialog.open(config.component, dialogConfig);

    return dialogRef.afterClosed();
  }
}
