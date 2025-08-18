import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { catchError, map, Observable, of, tap } from 'rxjs';
import { Result } from '../../shared/result.model';
import { Supplier } from './supplier.model';

@Injectable({
  providedIn: 'root',
})
export class SuppliersDataService {
  private readonly BASE_URL = 'api/suppliers';
  private http = inject(HttpClient);

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
