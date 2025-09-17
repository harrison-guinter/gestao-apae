import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface PageInfo {
  title: string;
  subtitle: string;
}

@Injectable({
  providedIn: 'root',
})
export class PageInfoService {
  private pageInfoSubject = new BehaviorSubject<PageInfo>({
    title: 'Dashboard',
    subtitle: 'Sistema de Gestão de Atendimentos',
  });

  public pageInfo$ = this.pageInfoSubject.asObservable();

  updatePageInfo(title: string, subtitle: string) {
    this.pageInfoSubject.next({ title, subtitle });
  }

  private routeMapping: { [key: string]: PageInfo } = {
    '/home/dashboard': { title: 'Dashboard', subtitle: 'Sistema de Gestão de Atendimentos' },
    '/home/assistidos': { title: 'Assistidos', subtitle: 'Gerenciar assistidos cadastrados' },
    '/home/convenios': { title: 'Convênios', subtitle: 'Gerenciar convênios e parcerias' },
    '/home/agendamentos': { title: 'Agendamentos', subtitle: 'Agendar e gerenciar consultas' },
    '/home/atendimentos': {
      title: 'Atendimentos',
      subtitle: 'Registrar e acompanhar atendimentos',
    },
    '/home/relatorios/atendimentos': {
      title: 'Relatórios de Atendimentos',
      subtitle: 'Relatórios e estatísticas de atendimentos',
    },
    '/home/relatorios/faltas': {
      title: 'Relatórios de Faltas',
      subtitle: 'Relatórios de faltas e ausências',
    },
    '/home/relatorios/individual': {
      title: 'Relatórios Individuais',
      subtitle: 'Relatórios por assistido',
    },
    '/home/relatorios/presencas': {
      title: 'Relatórios de Presenças',
      subtitle: 'Relatórios de presenças e frequência',
    },
    '/home/usuarios': { title: 'Usuários', subtitle: 'Gerenciar usuários do sistema' },
  };

  updatePageInfoByRoute(route: string) {
    const pageInfo = this.routeMapping[route];
    if (pageInfo) {
      this.pageInfoSubject.next(pageInfo);
    }
  }
}
