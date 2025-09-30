import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({ providedIn: 'root' })
export class NotificationService {
  public camposObrigatorios = 'Por favor, preencha todos os campos obrigatórios.';

  constructor(private snackBar: MatSnackBar) {}

  success(message: string, duration: number = 3000) {
    this.snackBar.open(message, 'Fechar', {
      panelClass: ['snackbar-success', 'success-snackbar'],
      duration,
      verticalPosition: 'bottom',
      horizontalPosition: 'center',
    });
  }

  fail(message: string, duration: number = 5000) {
    this.snackBar.open(message, 'Fechar', {
      panelClass: ['snackbar-error', 'error-snackbar'],
      duration,
      verticalPosition: 'bottom',
      horizontalPosition: 'center',
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
      verticalPosition: 'bottom',
      horizontalPosition: 'center',
    });
  }

  showInfo(message: string, duration: number = 3000): void {
    this.snackBar.open(message, 'Fechar', {
      panelClass: ['info-snackbar'],
      duration,
      verticalPosition: 'bottom',
      horizontalPosition: 'center',
    });
  }
}
