import { HttpHeaders, HttpParams } from '@angular/common/http';

export class HttpOptions {
  static withoutLoading(options?: {
    headers?: HttpHeaders | { [header: string]: string | string[] };
    params?:
      | HttpParams
      | { [param: string]: string | number | boolean | ReadonlyArray<string | number | boolean> };
  }): any {
    let headers = options?.headers;

    if (headers instanceof HttpHeaders) {
      headers = headers.set('X-Skip-Loading', 'true');
    } else {
      headers = {
        ...headers,
        'X-Skip-Loading': 'true',
      };
    }

    return {
      ...options,
      headers,
    };
  }
}
