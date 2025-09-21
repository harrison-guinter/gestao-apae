import { Injectable, ComponentRef, Type } from '@angular/core';
import { MatDialog, MatDialogRef, MatDialogConfig } from '@angular/material/dialog';
import { Observable } from 'rxjs';

export interface ModalConfig {
  component: Type<any>;
  title?: string;
  width?: string;
  height?: string;
  maxWidth?: string;
  maxHeight?: string;
  element?: any;
  data?: any;
  disableClose?: boolean;
  panelClass?: string | string[];
}

export interface ModalData {
  title?: string;
  element?: any;
  data?: any;
}

@Injectable({
  providedIn: 'root',
})
export class ModalService {
  constructor(private dialog: MatDialog) {}

  /**
   * Abre uma modal com o componente especificado
   * @param config Configurações da modal
   * @returns Observable que emite quando a modal é fechada
   */
  openModal<T = any, R = any>(config: ModalConfig): Observable<R | undefined> {
    const dialogConfig: MatDialogConfig = {
      width: config.width || '50%',
      height: config.height || 'auto',
      maxWidth: config.maxWidth || '90vw',
      maxHeight: config.maxHeight || '90vh',
      disableClose: config.disableClose || false,
      panelClass: config.panelClass || 'custom-modal-panel',
      data: {
        title: config.title,
        element: config.element,
        data: config.data,
      } as ModalData,
    };

    const dialogRef: MatDialogRef<T, R> = this.dialog.open(config.component, dialogConfig);

    return dialogRef.afterClosed();
  }

  /**
   * Abre uma modal de confirmação
   * @param title Título da modal
   * @param message Mensagem da modal
   * @param confirmText Texto do botão de confirmação
   * @param cancelText Texto do botão de cancelamento
   * @returns Observable<boolean> - true se confirmado, false se cancelado
   */
  openConfirmDialog(
    title: string,
    message: string,
    confirmText: string = 'Confirmar',
    cancelText: string = 'Cancelar'
  ): Observable<boolean> {
    // Implementação da modal de confirmação será criada separadamente
    return new Observable((observer) => {
      const confirmed = confirm(`${title}\n\n${message}`);
      observer.next(confirmed);
      observer.complete();
    });
  }

  /**
   * Fecha todas as modais abertas
   */
  closeAll(): void {
    this.dialog.closeAll();
  }

  /**
   * Verifica se há modais abertas
   */
  hasOpenDialogs(): boolean {
    return this.dialog.openDialogs.length > 0;
  }

  /**
   * Obtém a referência da modal atualmente aberta
   */
  getOpenDialogs(): MatDialogRef<any>[] {
    return this.dialog.openDialogs;
  }
}
