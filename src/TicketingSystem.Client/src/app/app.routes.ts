import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/tickets',
    pathMatch: 'full',
  },
  {
    path: 'tickets',
    loadComponent: () =>
      import('./features/ticket-dashboard/ticket-dashboard.component').then(
        (m) => m.TicketDashboardComponent
      ),
  },
  {
    path: '**',
    redirectTo: '/tickets',
  },
];
