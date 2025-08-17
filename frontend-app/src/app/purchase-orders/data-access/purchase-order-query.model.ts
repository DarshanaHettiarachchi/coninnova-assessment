import { PurchaseOrderStatus } from './purchase-order.model';

export type SortDirection = 'asc' | 'desc';

export interface PurchaseOrderQuery {
  supplierId?: string;
  status?: PurchaseOrderStatus;
  fromDate?: string;
  toDate?: string;
  sortBy?: string;
  sortDirection: SortDirection;
  page: number;
  pageSize: number;
}

export const DEFAULT_PURCHASE_ORDER_QUERY: PurchaseOrderQuery = {
  supplierId: undefined,
  status: undefined,
  fromDate: undefined,
  toDate: undefined,
  sortBy: 'PONUMBER', //fix hardcoded to match server expectations
  sortDirection: 'asc',
  page: 1,
  pageSize: 10,
};
