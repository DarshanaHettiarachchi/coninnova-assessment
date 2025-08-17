import {
  ChangeDetectionStrategy,
  Component,
  computed,
  input,
  output,
} from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSortModule, Sort, SortDirection } from '@angular/material/sort';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

export interface ColumnDef<T> {
  key: Extract<keyof T, string>;
  label: string;
  sortable?: boolean;
  cell?: (row: T) => string | number | Date;
}
@Component({
  selector: 'app-data-table',
  imports: [
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
  ],
  templateUrl: './data-table.component.html',
  styleUrl: './data-table.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DataTableComponent<T> {
  data = input.required<T[]>(); // Current page data
  columns = input.required<ColumnDef<T>[]>();
  totalItems = input.required<number>(); // Total items from server
  pageSize = input(10);
  pageIndex = input(0);
  sortActive = input<Extract<keyof T, string> | ''>('');
  sortDirection = input<SortDirection>('asc');

  pageChange = output<PageEvent>();
  sortChange = output<Sort>();

  displayedColumns = computed(() => this.columns().map((c) => c.key as string));

  onPageChange(event: PageEvent) {
    this.pageChange.emit(event);
  }

  onSortChange(event: Sort) {
    this.sortChange.emit(event);
  }
}
