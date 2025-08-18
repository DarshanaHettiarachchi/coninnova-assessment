import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { catchError, map, Observable, of, tap } from 'rxjs';
import { Supplier } from './supplier.model';
import { toSignal } from '@angular/core/rxjs-interop';
import { Result } from '../../shared/models/result.model';

@Injectable({
  providedIn: 'root',
})
export class SuppliersDataService {
  private readonly BASE_URL = 'api/suppliers';
  private http = inject(HttpClient);

  private readonly suppliersResponse$: Observable<Result<Supplier[]>> =
    this.fetchAllSuppliers();

  private readonly supplierResult = toSignal(this.suppliersResponse$, {
    initialValue: {} as Result<Supplier[]>,
  });

  suppliers = () => {
    return this.supplierResult().data || [];
  };

  private fetchAllSuppliers(): Observable<Result<Supplier[]>> {
    return this.http.get<Supplier[]>(this.BASE_URL).pipe(
      map((s) => {
        return { success: true, data: s } as Result<Supplier[]>;
      }),
      tap((s) => {
        console.log(s.data);
      }),
      catchError((err) => {
        return of({
          success: false,
          data: null,
          error: 'Failed to fetch suppliers',
        } as Result<Supplier[]>);
      })
    );
  }
}
