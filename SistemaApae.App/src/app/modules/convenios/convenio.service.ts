import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { map, Observable, of } from 'rxjs';
import { Convenio } from './convenio';
import { Cidade } from '../cidades/cidade';

@Injectable({
  providedIn: 'root',
})
export class ConvenioService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  listarConvenios(): Observable<Convenio[]> {
    return this.http
      .get<{ Data: Convenio[] }>(`${this.baseUrl}Convenio/filter`)
      .pipe(map((r) => r.Data)) || of([]);
  }
}
