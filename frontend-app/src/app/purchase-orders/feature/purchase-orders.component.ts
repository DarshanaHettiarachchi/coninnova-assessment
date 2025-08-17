import { ChangeDetectionStrategy, Component, signal } from '@angular/core';
import {
  ColumnDef,
  DataTableComponent,
} from '../ui/data-table/data-table.component';
import { PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';

interface PurchaseOrder {
  number: number;
  title: string;
  state: string;
  created_at: Date;
}

@Component({
  selector: 'app-purchase-orders',
  imports: [DataTableComponent],
  templateUrl: './purchase-orders.component.html',
  styleUrl: './purchase-orders.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PurchaseOrdersComponent {
  orders = signal<PurchaseOrder[]>([]);
  totalItems = signal(0);
  pageSize = signal(10);
  pageIndex = signal(0);

  columns: ColumnDef<PurchaseOrder>[] = [
    { key: 'number', label: '#' },
    { key: 'title', label: 'Title' },
    { key: 'state', label: 'State' },
    {
      key: 'created_at',
      label: 'Created',
      cell: (row) => new Intl.DateTimeFormat().format(new Date(row.created_at)),
    },
  ];

  ngOnInit() {
    this.fetchData(0, 10, null);
  }

  onPageChange(event: PageEvent) {
    this.pageIndex.set(event.pageIndex);
    this.pageSize.set(event.pageSize);
    this.fetchData(event.pageIndex, event.pageSize, null);
  }

  onSortChange(event: Sort) {
    this.fetchData(this.pageIndex(), this.pageSize(), event);
  }

  fetchData(pageIndex: number, pageSize: number, sort: Sort | null) {
    const allOrders: PurchaseOrder[] = Array.from({ length: 200 }).map(
      (_, i) => ({
        number: i + 1,
        title: `Order ${i + 1}`,
        state: i % 2 === 0 ? 'Open' : 'Closed',
        created_at: new Date(),
      })
    );

    const start = pageIndex * pageSize;
    const paged = allOrders.slice(start, start + pageSize);

    this.orders.set(paged);
    this.totalItems.set(allOrders.length);
  }
}
