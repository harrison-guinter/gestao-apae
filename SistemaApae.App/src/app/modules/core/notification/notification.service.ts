import { Injectable } from "@angular/core";
import { MatSnackBar } from "@angular/material/snack-bar";
import { NotificationComponent } from "./notification.component";

@Injectable({ providedIn: 'root' })
export class NotificationService {
  constructor(private snackBar: MatSnackBar) {}

  success(message: string) {
    this.snackBar.openFromComponent(NotificationComponent, {
      data: {message: message, type: 'success'},
      duration: 300000,
      verticalPosition: 'top'
    });
  }
}