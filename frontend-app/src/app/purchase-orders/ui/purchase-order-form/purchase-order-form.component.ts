import {
  ChangeDetectionStrategy,
  Component,
  computed,
  effect,
  inject,
  input,
  output,
} from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import {
  CreatePurchaseOrderRequest,
  PurchaseOrder,
  updatePurchaseOrderRequest,
} from '../../data-access/purchase-order.model';
import { MatNativeDateModule } from '@angular/material/core';
import { Supplier } from '../../data-access/supplier.model';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-purchase-order-form',
  imports: [
    MatDialogModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatCardModule,
  ],
  providers: [DatePipe],
  templateUrl: './purchase-order-form.component.html',
  styleUrl: './purchase-order-form.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PurchaseOrderFormComponent {
  fb = inject(FormBuilder);
  datePipe = inject(DatePipe);

  suppliers = input<Supplier[]>([]);
  poToEdit = input<updatePurchaseOrderRequest | null>(null);
  cancelEdit = output();
  addPO = output<CreatePurchaseOrderRequest>();
  updatePO = output<PurchaseOrder>();
  formLoading = input<boolean>(false);

  formTitle = computed(() =>
    this.poToEdit() ? 'Edit Purchase Order' : 'Add Purchase Order'
  );
  submitButtonText = computed(() => (this.poToEdit() ? 'Update' : 'Add'));

  form = this.fb.group({
    description: this.fb.control('', {
      validators: [Validators.required, Validators.maxLength(500)],
    }),
    supplier: this.fb.control<string>('', {
      validators: [Validators.required],
    }),
    orderDate: this.fb.control<Date | null>(null, {
      validators: [Validators.required],
    }),
    totalAmount: this.fb.control<number | null>(null, {
      validators: [Validators.required, Validators.min(1)],
    }),
  });

  constructor() {
    effect(() => {
      const po = this.poToEdit();
      if (po) {
        this.form.patchValue({
          description: po.description,
          supplier: po.supplierId,
          orderDate: new Date(po.orderDate),
          totalAmount: po.totalAmount,
        });

        console.log('Form patched with PO to edit:', this.form.value);
      }
    });
  }

  oncancel() {
    if (this.poToEdit()) {
      this.cancelEdit.emit();
    }
  }

  onsubmit() {
    this.form.markAllAsTouched();
    if (this.form.invalid) return;

    const formValue = this.form.value;

    const isoDate = this.datePipe.transform(formValue.orderDate, 'yyyy-MM-dd');

    const po: CreatePurchaseOrderRequest = {
      description: formValue.description as string,
      supplierId: formValue.supplier as string,
      orderDate: isoDate as string,
      totalAmount: formValue.totalAmount as number,
    };

    this.addPO.emit(po);
  }
}
