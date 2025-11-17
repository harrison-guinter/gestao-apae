import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { map, Observable, of } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { RelatorioFaltas } from './relatorio-faltas.interface';

export interface FaltasFiltro {
  dataInicio?: string;
  dataFim?: string;
  idAssistido?: string;
  idProfissional?: string;
  idMunicipio: string;
  idConvenio?: string;
}

@Injectable({
  providedIn: 'root',
})
export class RelatorioFaltasService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  listarFaltas(filtro: FaltasFiltro): Observable<RelatorioFaltas[]> {
    // Remove propriedades null ou undefined
    const params = Object.entries(filtro)
      .filter(([_, value]) => value !== null && value !== undefined && value !== '')
      .reduce((acc, [key, value]) => ({ ...acc, [key]: value }), {});
    console.log(1);
    return (
      this.http
        .get<{ data: RelatorioFaltas[] }>(`${this.baseUrl}Atendimento/reports/faltas`, {
          params: params as any,
        })
        .pipe(map((r) => r.data)) || of([])
    );
  }
}
