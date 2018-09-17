import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DefaultComponent } from './components/common/default/default.component';
import { NotFoundComponent } from './components/common/not-found/not-found.component';
import { RegisterComponent } from './components/user/register/register.component';
import { MainPageComponent } from './components/user/main-page/main-page.component';
import { FriendsComponent } from './components/user/friends/friends.component';
import { SettingsComponent } from './components/user/settings/settings.component';
import { ProductsComponent } from './components/user/products/products.component';

const routes: Routes = [
  { path: '', component: DefaultComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'user/mainpage', component: MainPageComponent},
  { path: 'user/friends', component: FriendsComponent},
  { path: 'user/products', component: ProductsComponent},
  { path: 'user/settings', component: SettingsComponent},
  { path: '**', component: NotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}
