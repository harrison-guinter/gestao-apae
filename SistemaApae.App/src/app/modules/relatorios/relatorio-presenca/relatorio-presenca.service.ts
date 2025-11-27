import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { map, Observable, of } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { RelatorioPresenca } from './relatorio-presenca.interface';

export interface PresencaFiltro {
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
export class RelatorioPresencasService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  listarPresenca(filtro: PresencaFiltro): Observable<RelatorioPresenca[]> {
    const params = Object.entries(filtro)
      .filter(([_, value]) => value !== null && value !== undefined && value !== '')
      .reduce((acc, [key, value]) => ({ ...acc, [key]: value }), {});

    return (
      this.http
        .get<{ data: RelatorioPresenca[] }>(`${this.baseUrl}Atendimento/reports/presencas`, {
          params: params as any,
        })
        .pipe(map((r) => r.data)) || of([])
    );
  }
}
