import { Component, OnDestroy } from '@angular/core';
import { PageEvent } from '@angular/material';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute, Params } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

import { SearchProduct } from '../../models/search-product';
import { SearchService } from '../../services/search/search.service';

@Component({
  selector: 'app-search-result',
  templateUrl: './search-result.component.html',
  styleUrls: ['./search-result.component.css']
})
export class SearchresultComponent implements OnDestroy {
  private destroy$ = new Subject<void>();

  query: string;
  searchResult: SearchProduct[];
  statusCode: number;
  tempProducts: SearchProduct[];
  errorMessage: string;
  pageSize = 5;
  pageIndex = 0;
  length = 0;

  constructor(private searchService: SearchService, private route: ActivatedRoute) {
    this.route.params.pipe(takeUntil(this.destroy$)).subscribe(({ query }: Params) => {
        if (!query) {
            return;
        }

        this.setVariablesToDefault();
        this.search(query);
    });
  }

  public ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  public search(query: string): void {
    this.searchService.getDefault(query).toPromise().then((data) => {
        this.searchResult = data;

        if (!this.searchResult) {
            return;
        }

        this.length = this.searchResult.length;
        this.changePageEvent();
    }).catch((e: HttpErrorResponse) => {
        this.errorMessage = 'Поиск не дал результатов...';
        if (e.status === 204) {
            this.statusCode = e.status;
        }
    });
}

  public changePageEvent(event?: PageEvent) {
    if (event != null) {
      this.pageIndex = event.pageIndex;
      this.pageSize = event.pageSize;
    }
    this.tempProducts = this.searchResult.slice(this.pageSize * this.pageIndex, this.pageSize * (this.pageIndex + 1));
  return event;
  }

  private setVariablesToDefault() {
    this.pageSize = 5;
    this.pageIndex = 0;
    this.length = 0;
  }
}
