import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({ providedIn: 'root' })
export class NotificationService {
  constructor(private snackBar: MatSnackBar) {}

  success(message: string, duration: number = 3000) {
    this.snackBar.open(message, 'Fechar', {
      panelClass: ['snackbar-success', 'success-snackbar'],
      duration,
      verticalPosition: 'top',
      horizontalPosition: 'right',
    });
  }

  fail(message: string, duration: number = 5000) {
    this.snackBar.open(message, 'Fechar', {
      panelClass: ['snackbar-error', 'error-snackbar'],
      duration,
      verticalPosition: 'top',
      horizontalPosition: 'right',
    });
  }

  // Métodos adicionais para compatibilidade com o interceptor
  showError(message: string, duration: number = 5000): void {
    this.fail(message, duration);
  }

  showSuccess(message: string, duration: number = 3000): void {
    this.success(message, duration);
  }

  showWarning(message: string, duration: number = 4000): void {
    this.snackBar.open(message, 'Fechar', {
      panelClass: ['warning-snackbar'],
      duration,
      verticalPosition: 'top',
      horizontalPosition: 'right',
    });
  }

  showInfo(message: string, duration: number = 3000): void {
    this.snackBar.open(message, 'Fechar', {
      panelClass: ['info-snackbar'],
      duration,
      verticalPosition: 'top',
      horizontalPosition: 'right',
    });
  }
}
