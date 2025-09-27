import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { Usuario } from './usuario';
import { Roles } from '../auth/roles.enum';
import { ApiResponse } from '../core/models/api-response.model';

export interface UsuarioFiltro {
  nome?: string;
  email?: string;
  perfil?: Roles;
  status?: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class UsuarioService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  listarUsuarios(filtros: UsuarioFiltro): Observable<Usuario[]> {
    return this.http
      .get<ApiResponse<Usuario[]>>(`${this.baseUrl}Usuario/`, { params: filtros as any })
      .pipe(map((response) => response.data || []));
  }
}
