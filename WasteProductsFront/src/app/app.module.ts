import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';


import { MaterialModule } from './modules/material/material.module';

/* Components */
import { AppComponent } from './app.component';
import { DatabaseComponent } from './components/database/database.component';

@NgModule({
  declarations: [
    AppComponent,

    DatabaseComponent
  ],
  imports: [
    BrowserModule, FormsModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MaterialModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
