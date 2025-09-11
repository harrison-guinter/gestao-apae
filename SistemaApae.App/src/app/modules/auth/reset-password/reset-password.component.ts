import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../auth.service';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-reset-password',
  imports: [ReactiveFormsModule, MatButtonModule, MatInputModule, MatIconModule],
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.less'],
})
export class ResetPasswordComponent {
  protected resetPasswordForm: FormGroup;

  constructor(private fb: FormBuilder, private authService: AuthService) {
    this.resetPasswordForm = this.fb.group({
      email: ['', Validators.required]
    });
  }

  resetPassword(): void {
    if (this.resetPasswordForm.invalid) return;
    const { email } = this.resetPasswordForm.value;
    this.authService.resetPassword(email).subscribe();
  }
}
