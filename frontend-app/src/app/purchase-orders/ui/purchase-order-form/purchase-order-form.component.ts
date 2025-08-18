import {
  ChangeDetectionStrategy,
  Component,
  computed,
  effect,
  inject,
  input,
  output,
} from '@angular/core';
import {
  FormBuilder,
  FormControl,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import {
  CreatePurchaseOrderRequest,
  PurchaseOrder,
  PurchaseOrderStatus,
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

  statuses = Object.keys(PurchaseOrderStatus).filter((k) => isNaN(Number(k)));

  form = this.fb.group<{
    description: FormControl<string>;
    supplier: FormControl<string>;
    orderDate: FormControl<Date | null>;
    totalAmount: FormControl<number | null>;
    status?: FormControl<string>;
  }>({
    description: this.fb.control('', {
      nonNullable: true,
      validators: [Validators.required, Validators.maxLength(500)],
    }),
    supplier: this.fb.control('', {
      nonNullable: true,
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
        this.form.addControl(
          'status',
          this.fb.control<string>('', {
            nonNullable: true,
            validators: [Validators.required],
          })
        );
        this.form.patchValue({
          description: po.description,
          supplier: po.supplierId,
          orderDate: new Date(po.orderDate),
          totalAmount: po.totalAmount,
          status: PurchaseOrderStatus[po.status],
        });
      } else {
        this.form.reset();
      }
    });
  }

  oncancel() {
    if (this.poToEdit()) {
      this.cancelEdit.emit();
      return;
    }
    this.form.reset();
  }

  onsubmit() {
    this.form.markAllAsTouched();
    if (this.form.invalid) return;

    const formValue = this.form.value;

    console.log('Form Value:', formValue);

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
