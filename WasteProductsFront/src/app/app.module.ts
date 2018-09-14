import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material';
import { MatButtonModule } from '@angular/material/button';
import { MaterialModule } from './modules/material/material.module';

/* Components */
import { AppComponent } from './app.component';
import { DatabaseComponent } from './components/database/database.component';
import { SearchComponent } from './components/search/component-search.component';
import { SearchresultComponent } from './components/searchresult/component-searchresult.component';

@NgModule({
  declarations: [
    AppComponent,
    DatabaseComponent,
    SearchComponent,
    SearchresultComponent
  ],
  imports: [
    BrowserModule, 
	FormsModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MatFormFieldModule,
    MatIconModule,
    MatButtonModule,    
    MaterialModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
