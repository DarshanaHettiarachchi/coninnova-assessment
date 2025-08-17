import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'purchase-orders',
    pathMatch: 'full',
  },
  {
    path: 'purchase-orders',
    loadComponent: () =>
      import('./purchase-orders/feature/purchase-orders.component').then(
        (m) => m.PurchaseOrdersComponent
      ),
  },
];
