import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { MaterialModule } from './modules/material/material.module';

/* Components */
import { AppComponent } from './app.component';
import { DatabaseComponent } from './components/database/database.component';
import { SearchComponent } from './components/search/search.component';
import { SearchresultComponent } from './components/searchresult/search-result.component';
import { SearchTopQueriesComponent } from './components/search-top-queries/search-top-queries.component';

@NgModule({
  declarations: [
    AppComponent,
    DatabaseComponent,
    SearchComponent,
    SearchresultComponent,
    SearchTopQueriesComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MaterialModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
