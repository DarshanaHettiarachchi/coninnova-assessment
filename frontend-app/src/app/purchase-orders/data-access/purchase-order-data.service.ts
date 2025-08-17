import { HttpClient, HttpParams } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import {
  catchError,
  debounceTime,
  map,
  Observable,
  of,
  switchMap,
  tap,
} from 'rxjs';
import { Result } from '../../shared/result.model';
import { PaginatedResponse } from '../../shared/paginated-response.model';
import {
  PurchaseOrder,
  PurchaseOrderJson,
  PurchaseOrderTableRow,
} from './purchase-order.model';
import {
  DEFAULT_PURCHASE_ORDER_QUERY,
  PurchaseOrderQuery,
} from './purchase-order-query.model';
import { toObservable, toSignal } from '@angular/core/rxjs-interop';
import { SortDirection } from '@angular/material/sort';

@Injectable({
  providedIn: 'root',
})
export class PurchaseOrderDataService {
  private readonly BASE_URL = 'api/purchase-orders';
  private http = inject(HttpClient);

  private readonly purchaseOrderQuery = signal<PurchaseOrderQuery>(
    DEFAULT_PURCHASE_ORDER_QUERY
  );

  private purchaseOrderResponse$ = toObservable(this.purchaseOrderQuery).pipe(
    debounceTime(300),
    switchMap(() => {
      return this.fetchPurchaseOrders(this.purchaseOrderQuery());
    })
  );

  private purchaseOrderResult = toSignal(this.purchaseOrderResponse$, {
    initialValue: {} as Result<PaginatedResponse<PurchaseOrder>>,
  });

  fetchPurchaseOrdersError = computed(
    () => this.purchaseOrderResult().error || null
  );

  purchaseOrders = computed(() => {
    return this.purchaseOrderResult().data || null;
  });

  purchaseOrderTableRows = computed(() => {
    return this.purchaseOrders()?.items.map(PurchaseOrder.toTableRow) || [];
  });

  totalItems = computed(() => {
    return this.purchaseOrders()?.totalItems || 0;
  });

  pageSize = computed(() => {
    return this.purchaseOrderQuery().pageSize || 10;
  });

  pageIndex = computed(() => {
    return this.purchaseOrderQuery().page
      ? this.purchaseOrderQuery().page - 1
      : 0;
  });

  sortBy = computed(() => {
    return this.purchaseOrderQuery().sortBy;
  });

  sortDirection = computed(() => {
    return this.purchaseOrderQuery().sortDirection;
  });

  setPaging(pageIndex: number, pageSize: number) {
    this.purchaseOrderQuery.update((query) => ({
      ...query,
      page: pageIndex + 1,
      pageSize: pageSize,
    }));
  }

  setSorting(
    sortBy: keyof PurchaseOrderTableRow,
    sortDirection: SortDirection
  ) {
    this.purchaseOrderQuery.update((query) => ({
      ...query,
      sortBy: sortBy,
      sortDirection: sortDirection,
    }));
  }

  setFilter(filter: Partial<PurchaseOrderQuery>) {
    this.purchaseOrderQuery.update((query) => ({
      ...query,
      ...filter,
    }));
  }

  private fetchPurchaseOrders(
    query: PurchaseOrderQuery
  ): Observable<Result<PaginatedResponse<PurchaseOrder>>> {
    return this.http
      .get<PaginatedResponse<PurchaseOrderJson>>(this.BASE_URL, {
        params: {
          status: query.status,
          page: query.page,
          pageSize: query.pageSize,
          sortBy: query.sortBy,
          sortDirection: query.sortDirection,
        },
      })
      .pipe(
        map((p) => {
          const paginated: PaginatedResponse<PurchaseOrder> = {
            ...p,
            items: p.items.map(PurchaseOrder.fromJson),
          };
          return { data: paginated } as Result<
            PaginatedResponse<PurchaseOrder>
          >;
        }),
        tap((p) => {
          console.log(p.data);
        }),
        catchError((err) => {
          return of({
            data: {} as PaginatedResponse<PurchaseOrder>,
            error: 'Failed to fetch purchase orders',
          } as Result<PaginatedResponse<PurchaseOrder>>);
        })
      );
  }
}
