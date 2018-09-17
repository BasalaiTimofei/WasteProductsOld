import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';


import { MaterialModule } from './modules/material/material.module';

/* Components */
import { AppComponent } from './app.component';
import { DatabaseComponent } from './components/database/database.component';
import { MainPageComponent } from './components/user/main-page/main-page.component';
import { AppRoutingModule } from './/app-routing.module';
import { NotFoundComponent } from './components/common/not-found/not-found.component';
import { DefaultComponent } from './components/common/default/default.component';
import { FriendsComponent } from './components/user/friends/friends.component';
import { ProductsComponent } from './components/user/products/products.component';
import { GroupsComponent } from './components/user/groups/groups.component';
import { SettingsComponent } from './components/user/settings/settings.component';
import { RegisterComponent } from './components/user/register/register.component';

@NgModule({
  declarations: [
    AppComponent,

    DatabaseComponent,

    MainPageComponent,

    NotFoundComponent,

    DefaultComponent,

    FriendsComponent,

    ProductsComponent,

    GroupsComponent,

    SettingsComponent,

    RegisterComponent
  ],
  imports: [
    BrowserModule, FormsModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MaterialModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
