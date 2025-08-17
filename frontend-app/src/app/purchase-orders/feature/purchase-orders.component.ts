import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import {
  ColumnDef,
  DataTableComponent,
} from '../ui/data-table/data-table.component';
import { PageEvent } from '@angular/material/paginator';
import { Sort, SortDirection } from '@angular/material/sort';
import { PurchaseOrderTableRow } from '../data-access/purchase-order.model';
import { PurchaseOrderDataService } from '../data-access/purchase-order-data.service';
import {
  DEFAULT_PURCHASE_ORDER_QUERY,
  PurchaseOrderQuery,
} from '../data-access/purchase-order-query.model';
import { DataFiltersComponent } from '../ui/data-filters/data-filters.component';

@Component({
  selector: 'app-purchase-orders',
  imports: [DataTableComponent, DataFiltersComponent],
  templateUrl: './purchase-orders.component.html',
  styleUrl: './purchase-orders.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PurchaseOrdersComponent {
  private purchaseOrderDataService = inject(PurchaseOrderDataService);

  orders = this.purchaseOrderDataService.purchaseOrderTableRows;
  totalItems = this.purchaseOrderDataService.totalItems;
  pageSize = this.purchaseOrderDataService.pageSize;
  pageIndex = this.purchaseOrderDataService.pageIndex;

  columns: ColumnDef<PurchaseOrderTableRow>[] = [
    { key: 'poNumber', label: '#', sortable: true },
    { key: 'description', label: 'Description' },
    { key: 'supplier', label: 'Supplier' },
    { key: 'totalAmount', label: 'Total Amount', sortable: true },
    { key: 'orderDate', label: 'Order Date', sortable: true },
    { key: 'status', label: 'State' },
  ];

  sortActive: keyof PurchaseOrderTableRow =
    DEFAULT_PURCHASE_ORDER_QUERY.sortBy as keyof PurchaseOrderTableRow;
  sortDirection: SortDirection = DEFAULT_PURCHASE_ORDER_QUERY.sortDirection;

  onPageChange(event: PageEvent) {
    console.log('page event:', event);
    this.purchaseOrderDataService.setPaging(event.pageIndex, event.pageSize);
  }

  onSortChange(event: Sort) {
    console.log('Sort event:', event);
    this.purchaseOrderDataService.setSorting(
      event.active as keyof PurchaseOrderTableRow,
      event.direction
    );
  }

  onFilterChange($event: Partial<PurchaseOrderQuery>) {
    console.log('Filter event:', $event);
    this.purchaseOrderDataService.setFilter({
      ...$event,
    });
  }
}
