import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { Usuario, StatusUsuarioEnum } from './usuario';
import { Roles } from '../auth/roles.enum';
import { ApiResponse } from '../core/models/api-response.model';

export interface UsuarioFiltro {
  nome?: string;
  email?: string;
  perfil?: Roles;
  status?: StatusUsuarioEnum;
}

@Injectable({
  providedIn: 'root',
})
export class UsuarioService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  listarUsuarios(filtros: UsuarioFiltro): Observable<Usuario[]> {
    const params = this.buildValidParams(filtros);

    return this.http
      .get<ApiResponse<Usuario[]>>(`${this.baseUrl}Usuario/filter`, { params })
      .pipe(map((response) => response.data || []));
  }

  private buildValidParams(filtros: UsuarioFiltro): any {
    const params: any = {};

    if (filtros.nome && filtros.nome.trim() !== '') {
      params.nome = filtros.nome.trim();
    }

    if (filtros.email && filtros.email.trim() !== '') {
      params.email = filtros.email.trim();
    }

    if (filtros.perfil !== undefined && filtros.perfil !== null) {
      params.perfil = filtros.perfil;
    }

    if (filtros.status !== undefined && filtros.status !== null) {
      params.status = filtros.status;
    }

    return params;
  }
}
