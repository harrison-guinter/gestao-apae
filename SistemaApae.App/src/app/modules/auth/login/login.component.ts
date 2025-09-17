import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../auth.service';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, MatButtonModule, MatInputModule, MatIconModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.less'],
})
export class LoginComponent {
  protected loginForm: FormGroup;

  constructor(private router: Router, private fb: FormBuilder, private authService: AuthService, private snackBar: MatSnackBar) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  login(): void {
    if (this.loginForm.invalid) return;
    const { email, password } = this.loginForm.value;
    this.authService.login(email, password).subscribe(
      (res) => {
        this.snackBar.open('Login realizado com sucesso', '', { duration: 3000 });
        this.router.navigate(['/home']);
      },
      (err) => this.snackBar.open('Erro ao realizar login: ' + err.error.message, '', { duration: 3000 })
    );
  }

  goToResetPassword(): void {
    this.router.navigate(['/reset-password']);
  }
}
