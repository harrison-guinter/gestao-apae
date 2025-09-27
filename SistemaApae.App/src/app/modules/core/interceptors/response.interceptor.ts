import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpResponse,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class ResponseInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      map((event: HttpEvent<any>) => {
        if (event instanceof HttpResponse && event.body) {
          // Log para desenvolvimento - mostra antes e depois da normalização
          console.log('Response original:', event.body);

          // Normalizar as chaves do body da resposta
          const normalizedBody = this.normalizeKeys(event.body);

          console.log('Response normalizado:', normalizedBody);

          return event.clone({
            body: normalizedBody,
          });
        }
        return event;
      })
    );
  }

  /**
   * Normaliza as chaves de um objeto convertendo a primeira letra para minúscula
   * Funciona recursivamente com objetos aninhados e arrays
   */
  private normalizeKeys(obj: any): any {
    if (obj === null || obj === undefined) {
      return obj;
    }

    if (Array.isArray(obj)) {
      // Se for array, normaliza cada item
      return obj.map((item) => this.normalizeKeys(item));
    }

    if (typeof obj === 'object' && obj.constructor === Object) {
      // Se for objeto, normaliza as chaves
      const normalized: any = {};

      for (const key in obj) {
        if (obj.hasOwnProperty(key)) {
          // Converte primeira letra para minúscula
          const normalizedKey = this.toCamelCase(key);
          // Recursivamente normaliza o valor se for objeto
          normalized[normalizedKey] = this.normalizeKeys(obj[key]);
        }
      }

      return normalized;
    }

    // Se não for objeto nem array, retorna o valor original
    return obj;
  }

  /**
   * Converte a primeira letra de uma string para minúscula
   */
  private toCamelCase(str: string): string {
    if (!str || str.length === 0) {
      return str;
    }

    return str.charAt(0).toLowerCase() + str.slice(1);
  }
}
