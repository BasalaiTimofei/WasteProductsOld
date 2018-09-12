import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { SearchComponent } from './components/search/component-search.component.component';
import { SearchresultComponent } from './components/searchresult/component-searchresult.component';

@NgModule({
  declarations: [
    AppComponent,
    SearchComponent,
    SearchresultComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
