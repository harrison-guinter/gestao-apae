import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { Usuario } from './usuario';
import { Roles } from '../auth/roles.enum';
import { ApiResponse } from '../core/models/api-response.model';
import { Status } from '../core/enum/status.enum';

export interface UsuarioFiltro {
  nome?: string;
  email?: string;
  perfil?: Roles;
  status?: Status;
}

@Injectable({
  providedIn: 'root',
})
export class UsuarioService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  filtrarUsuarios(filtros: UsuarioFiltro): Observable<Usuario[]> {
    const params = this.buildValidParams(filtros);

    return this.http.get<ApiResponse<Usuario[]>>(`${this.baseUrl}Usuario/filter`, { params }).pipe(
      map((response) => {
        const usuarios = response as any;
        return usuarios.sort((a: { nome: any }, b: { nome: any }) =>
          (a.nome || '').toLowerCase().localeCompare((b.nome || '').toLowerCase())
        );
      })
    );
  }

  salvarUsuario(usuario: Usuario): Observable<void> {
    const payload = this.buildUsuarioPayload(usuario, true);

    return this.http.post<void>(`${this.baseUrl}Usuario`, payload);
  }

  editarUsuario(usuario: Usuario): Observable<void> {
    const payload = this.buildUsuarioPayload(usuario, false);

    return this.http.put<void>(`${this.baseUrl}Usuario`, payload);
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

  private buildUsuarioPayload(usuario: Usuario, isCreating: boolean): any {
    const payload: any = {
      nome: usuario.nome?.trim() || '',
      email: usuario.email?.trim() || '',
      perfil: usuario.perfil || Roles.PROFISSIONAL,
      status: usuario.status || Status.Ativo,
    };

    // Campos opcionais
    if (usuario.telefone && usuario.telefone.trim()) {
      payload.telefone = usuario.telefone.trim();
    }

    if (usuario.registroProfissional && usuario.registroProfissional.trim()) {
      payload.registroProfissional = usuario.registroProfissional.trim();
    }

    if (usuario.especialidade && usuario.especialidade.trim()) {
      payload.especialidade = usuario.especialidade.trim();
    }

    if (usuario.observacao && usuario.observacao.trim()) {
      payload.observacao = usuario.observacao.trim();
    }

    // Se está editando, inclui o ID
    if (!isCreating && usuario.id) {
      payload.id = usuario.id;
    }

    return payload;
  }
}
