import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { Usuario } from '../usuarios/usuario';
import { Roles } from './roles.enum';


@Injectable({
  providedIn: 'root'
})
export class ProfissionalGuard implements CanActivate {
  constructor() {}

  public canActivate(): boolean {

     return !!(Usuario.getCurrentUser()).hasRole(Roles.PROFISSIONAL); 
  }
  
}