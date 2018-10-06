import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { GroupsComponent } from './components/groups/groups.component';
import { GroupComponent } from './components/group/group.component';


const routes: Routes = [
  { path: '', component: GroupsComponent, pathMatch: 'full'},
  { path: ':id', component: GroupComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GroupsRoutingModule { }
