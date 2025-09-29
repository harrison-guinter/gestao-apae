import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { LoadingService } from '../services/loading.service';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {
  private activeRequests = 0;

  constructor(private loadingService: LoadingService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Verifica se a requisição deve mostrar loading (por padrão sim)
    const showLoading = !req.headers.has('X-Skip-Loading');

    if (showLoading) {
      // Incrementa o contador de requisições ativas
      this.activeRequests++;

      // Mostra o loading se for a primeira requisição
      if (this.activeRequests === 1) {
        this.loadingService.show();
      }
    }

    return next.handle(req).pipe(
      finalize(() => {
        if (showLoading) {
          // Decrementa o contador ao finalizar a requisição
          this.activeRequests--;

          // Esconde o loading se não há mais requisições ativas
          if (this.activeRequests === 0) {
            this.loadingService.hide();
          }
        }
      })
    );
  }
}
