import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, firstValueFrom, map, Observable, of } from 'rxjs';
import { environment } from '../../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  login(email: string, password: string): Observable<boolean> {
    return this.http.post<{ token: string }>(`${this.baseUrl}/auth/login`, { email, password }).pipe(
      map(response => {
        if (response && response.token) {
          localStorage.setItem('token', response.token);
          return true;
        }
        return false;
      }),
      catchError(() => of(false))
    );
  }

  resetPassword(email: string): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/auth/reset-password`, { email })
  }

  logout() {
    localStorage.removeItem('token');
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('token');
  }
}