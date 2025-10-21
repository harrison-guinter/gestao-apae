import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { NotificationService } from '../notification/notification.service';
import { AuthService } from '../../auth/auth.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(
    private notificationService: NotificationService,
    private router: Router,
    private authService: AuthService
  ) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMessage = 'Erro desconhecido';

        if (error.error instanceof ErrorEvent) {
          // Erro do lado do cliente
          errorMessage = `Erro: ${error.error.message}`;
        } else {
          // Verificar erro 401 primeiro (não autorizado)
          if (error.status === 401) {
            errorMessage = 'Sessão expirada. Redirecionando para login...';
            this.authService.logout();
            this.router.navigate(['/login']);
            this.notificationService.showError(errorMessage);
            return throwError(() => error);
          }

          // Verificar se a resposta tem o formato da API (com Success, Message, Errors)
          const apiError = error.error;

          if (apiError && typeof apiError === 'object') {
            // Se tem Message da API, usar ela
            if (apiError.Message || apiError.message) {
              errorMessage = apiError.Message || apiError.message;
            }
            // Se tem Errors array, usar a primeira mensagem
            else if (
              (apiError.Errors && apiError.Errors.length > 0) ||
              (apiError.errors && apiError.errors.length > 0)
            ) {
              errorMessage = apiError.Errors?.[0] || apiError.errors?.[0] || errorMessage;
            }
          }

          // Fallback para códigos de status HTTP
          if (errorMessage === 'Erro desconhecido') {
            switch (error.status) {
              case 0:
                errorMessage = 'Erro de conexão. Verifique sua internet.';
                break;
              case 400:
                errorMessage = 'Dados inválidos';
                break;
              case 401:
                errorMessage = 'Sessão expirada. Redirecionando para login...';
                // Fazer logout e redirecionar para login
                this.authService.logout();
                this.router.navigate(['/login']);
                break;
              case 403:
                errorMessage = 'Acesso negado';
                break;
              case 404:
                errorMessage = 'Recurso não encontrado';
                break;
              case 422:
                errorMessage = 'Dados inválidos';
                break;
              case 500:
                errorMessage = 'Erro interno do servidor';
                break;
              case 409:
                errorMessage = error.error;
                break;
              default:
                errorMessage = `Erro ${error.status}: ${error.error}`;
            }
          }
        }

        // Exibir notificação de erro
        this.notificationService.showError(errorMessage);

        // Log do erro no console para desenvolvimento
        console.error('HTTP Error:', {
          status: error.status,
          statusText: error.statusText,
          url: error.url,
          error: error.error,
          message: errorMessage,
        });

        return throwError(() => error);
      })
    );
  }
}
