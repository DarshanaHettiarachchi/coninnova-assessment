import {
  ChangeDetectionStrategy,
  Component,
  effect,
  inject,
  input,
  output,
} from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { PurchaseOrderStatus } from '../../data-access/purchase-order.model';
import { toSignal } from '@angular/core/rxjs-interop';
import { PurchaseOrderQuery } from '../../data-access/purchase-order-query.model';
import { Supplier } from '../../data-access/supplier.model';

@Component({
  selector: 'app-data-filters',
  imports: [MatCardModule, MatIconModule, MatButtonModule, ReactiveFormsModule],
  templateUrl: './data-filters.component.html',
  styleUrl: './data-filters.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DataFiltersComponent {
  fb = inject(FormBuilder);

  form = this.fb.group({
    status: [''],
    supplier: [''],
  });

  statuses = Object.keys(PurchaseOrderStatus).filter((k) => isNaN(Number(k)));
  suppliers = input<Supplier[]>([]);

  filterChange = output<Partial<PurchaseOrderQuery>>();

  filterValues = toSignal(this.form.valueChanges);

  constructor() {
    effect(() => {
      const values = this.filterValues();
      if (values) {
        this.filterChange.emit({
          status:
            PurchaseOrderStatus[
              values.status as keyof typeof PurchaseOrderStatus
            ],
          supplierId: values.supplier,
        } as Partial<PurchaseOrderQuery>);
      }
    });
  }
}
