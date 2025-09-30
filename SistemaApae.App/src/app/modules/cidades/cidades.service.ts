import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Cidade } from './cidade';
import { map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CidadesService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  listarCidades(): Observable<Cidade[]> {
    return this.http
      .get<{ data: Cidade[] }>(`${this.baseUrl}Municipio/filter`)
      .pipe(map((r) => r.data));
  }
}
