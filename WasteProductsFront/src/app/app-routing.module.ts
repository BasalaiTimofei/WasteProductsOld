import { NgModule } from '@angular/core';
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

const routes: Routes = [
  { path: '', component: DefaultComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'common/mainpage', component: MainPageComponent},
  { path: 'user/friends', component: FriendsComponent},
  { path: 'products', component: ProductsComponent},
  { path: 'groups', component: GroupsComponent},
  { path: 'user/settings', component: SettingsComponent},
  { path: '**', component: NotFoundComponent },
  { path: 'products/to-list', component: ToListComponent },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}
