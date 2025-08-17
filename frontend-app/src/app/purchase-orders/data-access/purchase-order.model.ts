export interface SupplierResponse {
  id: string;
  name: string;
}

export interface Money {
  amount: number;
  currency: string;
}

export enum PurchaseOrderStatus {
  Draft = 0,
  Approved = 1,
  Shipped = 2,
  Completed = 3,
  Cancelled = 4,
}

export class PurchaseOrder {
  constructor(
    public id: string,
    public poNumber: string,
    public description: string,
    public status: PurchaseOrderStatus,
    public orderDate: string,
    public totalAmount: Money,
    public supplier: SupplierResponse
  ) {}

  get formattedDate(): string {
    return new Date(this.orderDate).toLocaleDateString();
  }

  static fromJson(json: PurchaseOrderJson): PurchaseOrder {
    return new PurchaseOrder(
      json.id,
      json.poNumber,
      json.description,
      json.status,
      json.orderDate,
      json.totalAmount,
      json.supplier
    );
  }

  static toTableRow(po: PurchaseOrder): PurchaseOrderTableRow {
    return {
      id: po.id,
      poNumber: po.poNumber,
      description: po.description,
      status: PurchaseOrderStatus[po.status],
      orderDate: po.orderDate,
      totalAmount: `${po.totalAmount.amount} ${po.totalAmount.currency}`,
      supplier: po.supplier.name,
    };
  }
}

export interface PurchaseOrderTableRow {
  id: string;
  poNumber: string;
  description: string;
  status: string;
  orderDate: string;
  totalAmount: string;
  supplier: string;
  actions?: string;
}

export interface PurchaseOrderJson {
  id: string;
  poNumber: string;
  description: string;
  status: PurchaseOrderStatus;
  orderDate: string;
  totalAmount: Money;
  supplier: SupplierResponse;
}
