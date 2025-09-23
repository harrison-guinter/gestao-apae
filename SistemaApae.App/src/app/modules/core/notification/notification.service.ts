import { Injectable } from "@angular/core";
import { MatSnackBar } from "@angular/material/snack-bar";

@Injectable({ providedIn: 'root' })
export class NotificationService {
  constructor(private snackBar: MatSnackBar) {}

  success(message: string) {
    this.snackBar.open(message, "", {
      panelClass: ['snackbar-success'],
      duration: 300,
      verticalPosition: 'top'
    });
  }

  fail(message: string) {
    this.snackBar.open(message, "", {
      panelClass: ['snackbar-error'],
      duration: 300,
      verticalPosition: 'top'
    });
  }
}