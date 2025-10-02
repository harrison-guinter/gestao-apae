import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class RequestInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Adicionar token de autenticação se existir
    const token = localStorage.getItem('token');
    let authReq = req;

    if (token) {
      authReq = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
        },
      });
    }

    // 🔹 Normalizar parâmetros de query (GET requests)
    if (authReq.params.keys().length > 0) {
      let normalizedParams = authReq.params;

      authReq.params.keys().forEach((key) => {
        const value = authReq.params.get(key);

        // 🔹 remove se for null, undefined ou string vazia

        if (value === "null" || value === null || value === undefined || value === '') {
          normalizedParams = normalizedParams.delete(key);
          return;
        }

        const pascalKey = this.toPascalCase(key);
        if (pascalKey !== key) {
          normalizedParams = normalizedParams.delete(key);
          normalizedParams = normalizedParams.set(pascalKey, value);
        }
      });

      authReq = authReq.clone({
        params: normalizedParams,
      });
    }

    // 🔹 Normalizar body de requisições POST/PUT
    if ((authReq.method === 'POST' || authReq.method === 'PUT') && authReq.body) {
      const normalizedBody = this.normalizeKeysForBackend(authReq.body);

      authReq = authReq.clone({
        body: normalizedBody,
      });
    }

    return next.handle(authReq);
  }

  /**
   * Converte camelCase para PascalCase
   */
  private toPascalCase(str: string): string {
    if (!str || str.length === 0) {
      return str;
    }
    return str.charAt(0).toUpperCase() + str.slice(1);
  }

  /**
   * Normaliza chaves e remove valores null/undefined/"" de objetos e arrays
   */
  private normalizeKeysForBackend(obj: any): any {
    if (obj === null || obj === undefined || obj === '') {
      return undefined;
    }

    if (Array.isArray(obj)) {
      return obj
        .map((item) => this.normalizeKeysForBackend(item))
        .filter((item) => item !== undefined);
    }

    if (typeof obj === 'object' && obj.constructor === Object) {
      const normalized: any = {};

      for (const key in obj) {
        if (obj.hasOwnProperty(key)) {
          const value = this.normalizeKeysForBackend(obj[key]);
          if (value !== undefined) {
            const pascalKey = this.toPascalCase(key);
            normalized[pascalKey] = value;
          }
        }
      }

      return normalized;
    }

    return obj;
  }

  /**
   * Converte HttpParams para objeto (debug)
   */
  private paramsToObject(params: any): any {
    const obj: any = {};
    params.keys().forEach((key: string) => {
      obj[key] = params.get(key);
    });
    return obj;
  }
}
