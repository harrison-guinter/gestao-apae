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
    return of([new Convenio('id', 'nome', new Cidade('9d8f9bb7-1ae6-4e11-9f5d-196ce6647083', 'Alto Feliz'), 'obs', true) ]);
      // this.http
      // .get<{ data: Convenio[] }>(`${this.baseUrl}Convenio/all`)
      // .pipe(map((r) => r.data)) || 
      
  }
}
