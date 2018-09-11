import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { SearchComponent } from './components/search/search.component';
import { SearchresultComponent } from './components/searchresult/searchresult.component';

@NgModule({
  declarations: [
    AppComponent,
    SearchComponent,
    SearchresultComponent
  ],
  imports: [
    BrowserModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
