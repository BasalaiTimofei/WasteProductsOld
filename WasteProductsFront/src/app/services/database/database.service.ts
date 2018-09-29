import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

import { BaseHttpService } from '../base/base-http.service';
import { LoggingService } from '../logging/logging.service';

/* Models */
import { DatabaseState } from '../../models/database/database-state';

/* Environment */
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DatabaseService extends BaseHttpService {

  private apiUrl = `${environment.apiHostUrl}/api/administration/database`;  // URL to web api

  public constructor(httpService: HttpClient, loggingService: LoggingService) {
    super(httpService, loggingService);
   }

  public getState(): Observable<DatabaseState> {
    const url = `${this.apiUrl}/state`;

    return this.httpService.get<DatabaseState>(url)
    .pipe(
      tap(data => this.logDebug('fetched database state')),
      catchError(this.handleError('getState', new DatabaseState(false, false)))
      );
  }

  public reCreate(withTestData: boolean): Observable<any> {
    const url = `${this.apiUrl}/recreate?withTestData=${withTestData}`;

    return this.httpService.get(url)
    .pipe(
      tap(data => this.logDebug('fetched database state')),
      catchError(this.handleError('reCreate'))
    );
  }

  public delete(): Observable<any> {
    const url = `${this.apiUrl}/delete`;

    return this.httpService.delete(url)
    .pipe(
      tap(data => this.logDebug('delete action executed')),
      catchError(this.handleError('delete'))
    );
  }
}
