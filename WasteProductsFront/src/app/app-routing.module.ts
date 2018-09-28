import { NgModule, Component } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DefaultComponent } from './components/common/default/default.component';
import { NotFoundComponent } from './components/common/not-found/not-found.component';
import { RegisterComponent } from './components/user/register/register.component';
import { MainPageComponent } from './components/common/main-page/main-page.component';
import { FriendsComponent } from './components/user/friends/friends.component';
import { ProductsComponent } from './components/products/products.component';
import { GroupsComponent } from './components/groups/groups.component';
import { SettingsComponent } from './components/user/settings/settings.component';
import { ToListComponent } from './components/products/to-list/to-list.component';
import { SearchComponent } from './components/search/search.component';
import { SearchresultComponent } from './components/searchresult/search-result.component';

/* Account components */
import { AccountComponent } from './modules/account/components/account/account.component';
import { AccountRegisterComponent } from './modules/account/components/account-register/account-register.component';

/* Route guards */
import { AuthenticationGuard } from './modules/account/guards/authentication.guard';

/* Environment */
import { environment } from '../environments/environment';

const routes: Routes = [
  { path: '', component: DefaultComponent },
  {
    path: 'account',
    children: [
      {
        path: '',
        pathMatch: 'full',
        component: AccountComponent,
        canActivate: [AuthenticationGuard]
      },
      {
        path: 'register',
        component: AccountRegisterComponent
      },

    ]
  },
  { path: 'common/mainpage', component: MainPageComponent },
  { path: 'user/friends', component: FriendsComponent },
  { path: 'products', component: ProductsComponent },
  { path: 'groups', component: GroupsComponent },
  { path: 'user/settings', component: SettingsComponent },
  { path: 'products/to-list', component: ToListComponent },
  { path: 'searchresults/:query', component: SearchresultComponent },
  { path: '**', component: NotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {
    enableTracing: !environment.production,
  })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
