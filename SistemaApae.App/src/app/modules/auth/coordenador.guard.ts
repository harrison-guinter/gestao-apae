import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';


@Injectable({
  providedIn: 'root'
})
export class CoordenadorGuard implements CanActivate {
  constructor() {}

  public canActivate(): boolean {
    return !!JSON.parse(localStorage.getItem('usuario') || '{ "roles": [] }').roles.includes('Coordenador'); 
  }
  
}