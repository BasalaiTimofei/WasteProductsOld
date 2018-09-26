import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { MaterialModule } from './modules/material/material.module';
import { CdkTableModule } from '@angular/cdk/table';
import { OverlayModule } from '@angular/cdk/overlay';

/* Components */
import { AppComponent } from './app.component';
import { DatabaseComponent } from './components/database/database.component';
import { SearchComponent } from './components/search/search.component';
import { SearchresultComponent } from './components/searchresult/search-result.component';
import { MainPageComponent } from './components/common/main-page/main-page.component';
import { AppRoutingModule } from './/app-routing.module';
import { NotFoundComponent } from './components/common/not-found/not-found.component';
import { DefaultComponent } from './components/common/default/default.component';
import { FriendsComponent } from './components/user/friends/friends.component';
import { ProductsComponent } from './components/products/products.component';
import { GroupsComponent } from './components/groups/groups.component';
import { SettingsComponent } from './components/user/settings/settings.component';
import { RegisterComponent } from './components/user/register/register.component';
import { ToListComponent } from './components/products/to-list/to-list.component';
import { AddProductComponent } from './components/products/add-product/add-product.component';
import { DeleteProductComponent } from './components/products/delete-product/delete-product.component';
import { UpdateProductComponent } from './components/products/update-product/update-product.component';
import { HeaderComponent } from './components/common/header/header.component';
import { ImagePreviewComponent } from './components/image-preview/image-preview.component';

/* Services */
import { ImagePreviewService } from './services/image-preview/image-preview.service';
import { ImageOverlayWrapperComponent } from './components/image-preview/image-overlay-wrapper/image-overlay-wrapper.component';

@NgModule({
  declarations: [
    AppComponent,
    DatabaseComponent,
    SearchComponent,
    SearchresultComponent,
    MainPageComponent,
    NotFoundComponent,
    DefaultComponent,
    FriendsComponent,
    ProductsComponent,
    GroupsComponent,
    SettingsComponent,
    RegisterComponent,
    ToListComponent,
    AddProductComponent,
    DeleteProductComponent,
    UpdateProductComponent,
    HeaderComponent,
    ImagePreviewComponent,
    ImageOverlayWrapperComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MaterialModule,
    AppRoutingModule,
    CdkTableModule,
    OverlayModule
  ],
  providers: [ImagePreviewService],
  entryComponents: [ImagePreviewComponent],
  bootstrap: [AppComponent]
})
export class AppModule { }
