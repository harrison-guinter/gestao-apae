import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { map, Observable, of } from 'rxjs';
import { Convenio } from './convenio';
import { Status } from '../core/enum/status.enum';

export interface ConvenioFiltro {
  Nome?: string;
  Status?: Status;
}

@Injectable({
  providedIn: 'root',
})
export class ConvenioService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  listarConvenios(filtro: ConvenioFiltro): Observable<Convenio[]> {
    return (
      this.http
        .get<{ data: Convenio[] }>(`${this.baseUrl}Convenio/filter`, { params: filtro as any })
        .pipe(map((r) => r.data)) || of([])
    );
  }

    salvar(convenio: Convenio): Observable<void> {  
      return this.http.post<void>(`${this.baseUrl}Convenio`, convenio);
    }
  
    editar(convenio: Convenio): Observable<void> {

      return this.http.put<void>(`${this.baseUrl}Convenio`, convenio);
    }
  
}
