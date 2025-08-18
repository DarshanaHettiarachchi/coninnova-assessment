import { HttpClient, HttpParams } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import {
  catchError,
  debounceTime,
  filter,
  map,
  Observable,
  of,
  switchMap,
  tap,
} from 'rxjs';
import {
  CreatePurchaseOrderRequest,
  PurchaseOrder,
  PurchaseOrderJson,
  PurchaseOrderTableRow,
  updatePurchaseOrderRequest,
} from './purchase-order.model';
import {
  DEFAULT_PURCHASE_ORDER_QUERY,
  PurchaseOrderQuery,
} from './purchase-order-query.model';
import { toObservable, toSignal } from '@angular/core/rxjs-interop';
import { SortDirection } from '@angular/material/sort';
import { Result } from '../../shared/models/result.model';
import { PaginatedResponse } from '../../shared/models/paginated-response.model';

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

  private pendingSave = signal<CreatePurchaseOrderRequest | null>(null);
  private pendingSave$ = toObservable(this.pendingSave);

  private savedResult$ = this.pendingSave$.pipe(
    filter(Boolean),
    switchMap((po) => {
      return this.addPurchaseOrder(po);
    })
  );

  private SavedResult = toSignal(this.savedResult$, {
    initialValue: { data: {} as PurchaseOrder } as Result<PurchaseOrder>,
  });

  private pendingEdit = signal<updatePurchaseOrderRequest | null>(null);
  poToEdit = this.pendingEdit.asReadonly();

  private pendingUpdate = signal<updatePurchaseOrderRequest | null>(null);
  private pendingUpdate$ = toObservable(this.pendingUpdate);
  private updatedResult$ = this.pendingUpdate$.pipe(
    filter(Boolean),
    switchMap((po) => {
      return this.updatePurchaseOrder(po);
    })
  );

  private UpdatedResult = toSignal(this.updatedResult$, {
    initialValue: { data: {} as PurchaseOrder } as Result<PurchaseOrder>,
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

  queueForSave(po: CreatePurchaseOrderRequest) {
    this.pendingSave.set(po);
  }

  queueForEdit(tr: PurchaseOrderTableRow | null) {
    if (!tr) {
      this.pendingEdit.set(null);
      return;
    }
    const po = this.purchaseOrders()?.items.find((po) => po.id === tr.id);
    if (!po) {
      console.error('Purchase Order not found for editing:', tr.id);
      return;
    }
    var ur = PurchaseOrder.toUpdateRequest(po);
    this.pendingEdit.set(ur);
  }

  queueForUpdate(po: updatePurchaseOrderRequest) {
    this.pendingUpdate.set(po);
  }

  private toParams(query: PurchaseOrderQuery): HttpParams {
    let params = new HttpParams()
      .set('page', query.page.toString())
      .set('pageSize', query.pageSize.toString())
      .set('sortBy', query.sortBy)
      .set('sortDirection', query.sortDirection);

    if (query.status) {
      params = params.set('status', query.status);
    }
    if (query.supplierId) {
      params = params.set('supplierId', query.supplierId);
    }

    return params;
  }

  private fetchPurchaseOrders(
    query: PurchaseOrderQuery
  ): Observable<Result<PaginatedResponse<PurchaseOrder>>> {
    var params = this.toParams(query);
    return this.http
      .get<PaginatedResponse<PurchaseOrderJson>>(this.BASE_URL, {
        params,
      })
      .pipe(
        map((p) => {
          const paginated: PaginatedResponse<PurchaseOrder> = {
            ...p,
            items: p.items.map(PurchaseOrder.fromJson),
          };
          return { success: true, data: paginated } as Result<
            PaginatedResponse<PurchaseOrder>
          >;
        }),
        tap((p) => {
          console.log(p.data);
        }),
        catchError((err) => {
          return of({
            success: false,
            data: null,
            error: 'Failed to fetch purchase orders',
          } as Result<PaginatedResponse<PurchaseOrder>>);
        })
      );
  }

  private addPurchaseOrder(
    po: CreatePurchaseOrderRequest
  ): Observable<Result<PurchaseOrder>> {
    return this.http.post<PurchaseOrderJson>(this.BASE_URL, po).pipe(
      map(
        (r) =>
          ({
            success: true,
            data: PurchaseOrder.fromJson(r),
          } as Result<PurchaseOrder>)
      ),
      tap((p) => {
        console.log(p.data);
      }),
      catchError((err) => {
        return of({
          success: false,
          data: null,
          error: 'Failed to save purchase order.',
        } as Result<PurchaseOrder>);
      })
    );
  }

  private updatePurchaseOrder(
    po: updatePurchaseOrderRequest
  ): Observable<Result<PurchaseOrder>> {
    return this.http
      .put<PurchaseOrderJson>(`${this.BASE_URL}/${po.id}`, po)
      .pipe(
        map(
          (r) =>
            ({
              success: true,
              data: PurchaseOrder.fromJson(r),
            } as Result<PurchaseOrder>)
        ),
        tap((p) => {
          console.log(p.data);
        }),
        catchError((err) => {
          return of({
            success: false,
            data: null,
            error: 'Failed to save purchase order.',
          } as Result<PurchaseOrder>);
        })
      );
  }
}
