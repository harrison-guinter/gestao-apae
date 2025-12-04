import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable, of } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { RelatorioIndividual } from './relatorio-individual.interface';

export interface RelatorioIndividualFiltro {
  dataInicio?: string;
  dataFim?: string;
  idAssistido?: string;
  idProfissional?: string;
  idMunicipio?: string;
  limit?: number;
  skip?: number;
}

@Injectable({
  providedIn: 'root',
})
export class RelatorioIndividualService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  listarAtendimentos(filtro: RelatorioIndividualFiltro): Observable<RelatorioIndividual[]> {
    const params = Object.entries(filtro)
      .filter(([_, value]) => value !== null && value !== undefined && value !== '')
      .reduce((acc, [key, value]) => ({ ...acc, [key]: value }), {});

    return (
      this.http
        .get<{ data: RelatorioIndividual[] }>(`${this.baseUrl}Atendimento/reports/assistidos`, {
          params: params as any,
        })
        .pipe(map((r) => r.data)) || of([])
    );
  }
}
