import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Usuario } from '../usuarios/usuario';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  login(email: string, password: string): Observable<boolean> {
    return this.http
      .post<{ message: string; data: { user: Usuario; token: string } }>(
        `${this.baseUrl}auth/login`,
        { email, password }
      )
      .pipe(
        map((response) => {
          if (response && response.data.token) {
            localStorage.setItem('token', response.data.token);
            localStorage.setItem('usuario', JSON.stringify(response.data.user));
            return true;
          }
          return false;
        })
      );
  }

  resetPassword(email: string): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${this.baseUrl}auth/forgot-password`, { email });
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('usuario');
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('token');
  }
}
