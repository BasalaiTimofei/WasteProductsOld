import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { MaterialModule } from './modules/material/material.module';
import { CdkTableModule } from '@angular/cdk/table';

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
import { GroupCreateComponent } from './components/groups/group/group-create/group-create.component';
import { GroupUpdateComponent } from './components/groups/group/group-update/group-update.component';
import { GroupDeleteComponent } from './components/groups/group/group-delete/group-delete.component';
import { GroupBoardCreateComponent } from './components/groups/board/group-board-create/group-board-create.component';
import { GroupBoardUpdateComponent } from './components/groups/board/group-board-update/group-board-update.component';
import { GroupBoardDeleteComponent } from './components/groups/board/group-board-delete/group-board-delete.component';
import { GroupProductCreateComponent } from './components/groups/product/group-product-create/group-product-create.component';
import { GroupProductUpdateComponent } from './components/groups/product/group-product-update/group-product-update.component';
import { GroupCommentCreateComponent } from './components/groups/comment/group-comment-create/group-comment-create.component';
import { GroupCommentUpdateComponent } from './components/groups/comment/group-comment-update/group-comment-update.component';
import { GroupCommentDeleteComponent } from './components/groups/comment/group-comment-delete/group-comment-delete.component';
import { GroupUserSendInviteComponent } from './components/groups/user/group-user-send-invite/group-user-send-invite.component';
import { GroupUserDismissUserComponent } from './components/groups/user/group-user-dismiss-user/group-user-dismiss-user.component';
import { GroupUserGetEntitleComponent } from './components/groups/user/group-user-get-entitle/group-user-get-entitle.component';

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
    GroupCreateComponent,
    GroupUpdateComponent,
    GroupDeleteComponent,
    GroupBoardCreateComponent,
    GroupBoardUpdateComponent,
    GroupBoardDeleteComponent,
    GroupProductCreateComponent,
    GroupProductUpdateComponent,
    GroupCommentCreateComponent,
    GroupCommentUpdateComponent,
    GroupCommentDeleteComponent,
    GroupUserSendInviteComponent,
    GroupUserDismissUserComponent,
    GroupUserGetEntitleComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MaterialModule,
    AppRoutingModule,
    CdkTableModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
