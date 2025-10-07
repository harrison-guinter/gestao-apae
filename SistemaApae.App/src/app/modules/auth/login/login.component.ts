import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../auth.service';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { NotificationService } from '../../core/notification/notification.service';
import { InputComponent } from '../../core/input/input.component';

@Component({
  selector: 'app-login',
  imports: [
    ReactiveFormsModule,
    MatButtonModule,
    MatInputModule,
    MatIconModule,
    MatCardModule,
    InputComponent,
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.less'],
})
export class LoginComponent {
  protected loginForm: FormGroup;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private authService: AuthService,
    private notificationService: NotificationService
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  login(): void {
    if (this.loginForm.invalid) return;
    const { email, password } = this.loginForm.value;
    this.authService.login(email, password).subscribe(
      () => {
        this.notificationService.success('Login realizado com sucesso');
        this.router.navigate(['/home']);
      },
      (err) => this.notificationService.fail('Erro ao realizar login: ' + err.error.message)
    );
  }

  goToResetPassword(): void {
    this.router.navigate(['/reset-password']);
  }
}
