import { Injectable } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { ConfirmationModalComponent, ConfirmationModalData } from './confirmation-modal.component';

export interface ConfirmationConfig {
  message: string;
  confirmButtonText?: string;
  cancelButtonText?: string;
  elementRef?: HTMLElement;
  disableClose?: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class ConfirmationService {
  public msgAlteracaoNaoSalva: string =
    'Existem alterações não salvas. Tem certeza que deseja continuar?';

  constructor(private dialog: MatDialog) {}

  /**
   * Abre uma modal de confirmação
   * @param config Configurações da modal
   * @returns Observable<boolean> - true se confirmado, false se cancelado
   */
  openConfirmationModal(config: ConfirmationConfig): Observable<boolean> {
    const dialogRef: MatDialogRef<ConfirmationModalComponent> = this.dialog.open(
      ConfirmationModalComponent,
      {
        width: 'auto',
        height: 'auto',
        minWidth: '400px',
        maxWidth: '500px',
        maxHeight: 'none',
        hasBackdrop: true,
        disableClose: config.disableClose !== undefined ? config.disableClose : true,
        panelClass: 'confirmation-dialog-container',
        position: this.calculatePosition(config.elementRef),
        autoFocus: true,
        restoreFocus: true,
        data: {
          message: config.message,
          confirmButtonText: config.confirmButtonText || 'Confirmar',
          cancelButtonText: config.cancelButtonText || 'Cancelar',
          elementRef: config.elementRef,
          disableClose: config.disableClose !== undefined ? config.disableClose : true,
        } as ConfirmationModalData,
      }
    );

    return dialogRef.afterClosed();
  }

  /**
   * Calcula a posição da modal próxima ao elemento referenciado
   */
  private calculatePosition(elementRef?: HTMLElement): { top?: string; left?: string } | undefined {
    if (!elementRef) {
      return undefined;
    }

    const rect = elementRef.getBoundingClientRect();
    const viewportHeight = window.innerHeight;
    const viewportWidth = window.innerWidth;

    // Posicionar próximo ao elemento, mas ajustar se sair da tela
    let top = rect.bottom + 10;
    let left = rect.left;

    // Ajustar se a modal sair da parte inferior da tela
    if (top + 200 > viewportHeight) {
      top = rect.top - 210; // Posicionar acima do elemento
    }

    // Ajustar se a modal sair da lateral direita da tela
    if (left + 400 > viewportWidth) {
      left = viewportWidth - 420; // Ajustar para caber na tela
    }

    // Garantir que não saia da lateral esquerda
    if (left < 20) {
      left = 20;
    }

    // Garantir que não saia do topo
    if (top < 20) {
      top = 20;
    }

    return {
      top: `${top}px`,
      left: `${left}px`,
    };
  }

  /**
   * Método de conveniência para confirmações simples
   */
  confirm(
    message: string,
    confirmText: string = 'Confirmar',
    cancelText: string = 'Cancelar'
  ): Observable<boolean> {
    return this.openConfirmationModal({
      message,
      confirmButtonText: confirmText,
      cancelButtonText: cancelText,
    });
  }

  /**
   * Método de conveniência para confirmações de exclusão
   */
  confirmDelete(itemName: string = 'este item', elementRef?: HTMLElement): Observable<boolean> {
    return this.openConfirmationModal({
      message: `Tem certeza que deseja excluir ${itemName}? Esta ação não poderá ser desfeita.`,
      confirmButtonText: 'Excluir',
      cancelButtonText: 'Cancelar',
      elementRef,
      disableClose: true,
    });
  }
}
