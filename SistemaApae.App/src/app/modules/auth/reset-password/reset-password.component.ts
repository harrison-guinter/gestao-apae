import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../auth.service';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-reset-password',
  imports: [ReactiveFormsModule, MatButtonModule, MatInputModule, MatIconModule],
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.less'],
})
export class ResetPasswordComponent {
  protected resetPasswordForm: FormGroup;

  constructor(private fb: FormBuilder, private authService: AuthService, private snackBar: MatSnackBar, private router: Router) {
    this.resetPasswordForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
    });
  }

  resetPassword(): void {
    if (this.resetPasswordForm.invalid) return;
    const { email } = this.resetPasswordForm.value;
    this.authService.resetPassword(email).subscribe(res => {
      this.snackBar.open(res.message, '', { duration: 3000 });
    });
  }

  goToLogin(): void {
    this.router.navigate(['/login']);
  }
}
