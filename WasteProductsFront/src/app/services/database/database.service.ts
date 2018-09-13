import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import { DatabaseState } from '../../models/database/database-state';
import { LoggingService } from '../logging/logging.service';
import { environment } from '../../../environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class DatabaseService {

  private apiUrl = `${environment.apiHostUrl}/api/administration/database`;  // URL to web api

  constructor(
    private http: HttpClient,
    private logService: LoggingService) { }

  getState(): Observable<DatabaseState> {
    const url = `${this.apiUrl}/state`;

    return this.http.get<DatabaseState>(url)
    .pipe(
      tap(data  => this.log('fetched database state'))
    );
  }

  reCreate(withTestData: boolean): Observable<any> {
    const url = `${this.apiUrl}/recreate?withTestData=${withTestData}`;

    return this.http.get(url)
    .pipe(
      tap(data => this.log('recreate action executed')),
    );
  }

  delete(): Observable<any> {
    const url = `${this.apiUrl}/delete`;

    return this.http.delete(url)
    .pipe(
      tap(data => this.log('delete action executed')),
    );
  }

  /** Log with the LoggingService */
  private log(msg: any) {
    this.logService.log(`DatabaseService: ${msg}`);
  }
}
