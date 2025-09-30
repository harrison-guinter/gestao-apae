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
    // Normalizar parâmetros de query (GET requests)
    if (authReq.method === 'GET' && authReq.params.keys().length > 0) {
      let normalizedParams = authReq.params;

      // Converter cada parâmetro de camelCase para PascalCase
      authReq.params.keys().forEach((key) => {
        const pascalKey = this.toPascalCase(key);
        const value = authReq.params.get(key);

        if (pascalKey !== key && value !== null) {
          // Remove o parâmetro com chave original
          normalizedParams = normalizedParams.delete(key);
          // Adiciona com chave em PascalCase
          normalizedParams = normalizedParams.set(pascalKey, value);
        }
      });

      // Cria uma nova requisição com parâmetros normalizados
      const normalizedRequest = authReq.clone({
        params: normalizedParams,
      });


      return next.handle(normalizedRequest);
    }

    // Normalizar body de requisições POST/PUT
    if ((authReq.method === 'POST' || authReq.method === 'PUT') && authReq.body) {
      const normalizedBody = this.normalizeKeysForBackend(authReq.body);

      const normalizedRequest = authReq.clone({
        body: normalizedBody,
      });


      return next.handle(normalizedRequest);
    }

    return next.handle(authReq);
  }

  /**
   * Converte camelCase para PascalCase
   * Exemplo: "nome" -> "Nome", "registroProfissional" -> "RegistroProfissional"
   */
  private toPascalCase(str: string): string {
    if (!str || str.length === 0) {
      return str;
    }

    return str.charAt(0).toUpperCase() + str.slice(1);
  }

  /**
   * Normaliza as chaves de um objeto para o formato esperado pelo backend (PascalCase)
   * Funciona recursivamente com objetos aninhados e arrays
   */
  private normalizeKeysForBackend(obj: any): any {
    if (obj === null || obj === undefined) {
      return obj;
    }

    if (Array.isArray(obj)) {
      // Se for array, normaliza cada item
      return obj.map((item) => this.normalizeKeysForBackend(item));
    }

    if (typeof obj === 'object' && obj.constructor === Object) {
      // Se for objeto, normaliza as chaves
      const normalized: any = {};

      for (const key in obj) {
        if (obj.hasOwnProperty(key)) {
          // Converte para PascalCase
          const pascalKey = this.toPascalCase(key);
          // Recursivamente normaliza o valor se for objeto
          normalized[pascalKey] = this.normalizeKeysForBackend(obj[key]);
        }
      }

      return normalized;
    }

    // Se não for objeto nem array, retorna o valor original
    return obj;
  }

  /**
   * Converte HttpParams para objeto para logging
   */
  private paramsToObject(params: any): any {
    const obj: any = {};
    params.keys().forEach((key: string) => {
      obj[key] = params.get(key);
    });
    return obj;
  }
}
