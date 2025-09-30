import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { Usuario } from '../usuarios/usuario';
import { Roles } from './roles.enum';

@Injectable({
  providedIn: 'root'
})
export class CoordenadorGuard implements CanActivate {
  constructor() {
  }

  public canActivate(): boolean {
    return !!(Usuario.getCurrentUser()).hasRole(Roles.COORDENADOR); 
  }
  
}