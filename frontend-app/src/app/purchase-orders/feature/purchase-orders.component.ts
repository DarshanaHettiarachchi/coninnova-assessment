import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import {
  ColumnDef,
  DataTableComponent,
} from '../ui/data-table/data-table.component';
import { PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { PurchaseOrderTableRow } from '../data-access/purchase-order.model';
import { PurchaseOrderDataService } from '../data-access/purchase-order-data.service';

@Component({
  selector: 'app-purchase-orders',
  imports: [DataTableComponent],
  templateUrl: './purchase-orders.component.html',
  styleUrl: './purchase-orders.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PurchaseOrdersComponent {
  private todoDataService = inject(PurchaseOrderDataService);

  orders = this.todoDataService.purchaseOrderTableRows;
  totalItems = this.todoDataService.totalItems;
  pageSize = this.todoDataService.pageSize;
  pageIndex = this.todoDataService.pageIndex;

  columns: ColumnDef<PurchaseOrderTableRow>[] = [
    { key: 'poNumber', label: '#' },
    { key: 'description', label: 'Title' },
    { key: 'supplier', label: 'Supplier' },
    { key: 'totalAmount', label: 'Total Amount' },
    { key: 'orderDate', label: 'Order Date' },
    { key: 'status', label: 'State' },
  ];

  onPageChange(event: PageEvent) {
    console.log('page event:', event);
    this.todoDataService.setPaging(event.pageIndex, event.pageSize);
  }

  onSortChange(event: Sort) {
    console.log('Sort event:', event);
  }
}
