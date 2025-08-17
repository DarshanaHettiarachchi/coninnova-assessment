import { SortDirection } from '@angular/material/sort';
import { PurchaseOrderStatus } from './purchase-order.model';

export interface PurchaseOrderQuery {
  supplierId: string;
  status: PurchaseOrderStatus | '';
  fromDate: string;
  toDate: string;
  sortBy: string;
  sortDirection: SortDirection;
  page: number;
  pageSize: number;
}

export const DEFAULT_PURCHASE_ORDER_QUERY: PurchaseOrderQuery = {
  supplierId: '',
  status: '',
  fromDate: '',
  toDate: '',
  sortBy: 'poNumber',
  sortDirection: 'desc',
  page: 1,
  pageSize: 10,
};
