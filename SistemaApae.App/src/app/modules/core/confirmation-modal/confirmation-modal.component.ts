import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ConfirmationService } from './confirmation.service';

export interface ConfirmationModalData {
  message: string;
  confirmButtonText?: string;
  cancelButtonText?: string;
  elementRef?: HTMLElement;
  disableClose?: boolean;
}

@Component({
  selector: 'app-confirmation-modal',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule, MatIconModule],
  templateUrl: './confirmation-modal.component.html',
  styleUrls: ['./confirmation-modal.component.less'],
})
export class ConfirmationModalComponent {
  constructor(
    public dialogRef: MatDialogRef<ConfirmationModalComponent>,
    public confirmationService: ConfirmationService,
    @Inject(MAT_DIALOG_DATA) public data: ConfirmationModalData
  ) {
    // Configurar disableClose se fornecido
    if (data.disableClose !== undefined) {
      this.dialogRef.disableClose = data.disableClose;
    }
  }

  get message(): string {
    return this.data.message;
  }

  get confirmButtonText(): string {
    return this.data.confirmButtonText || 'Confirmar';
  }

  get cancelButtonText(): string {
    return this.data.cancelButtonText || 'Cancelar';
  }

  onConfirm(): void {
    this.dialogRef.close(true);
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }
}
