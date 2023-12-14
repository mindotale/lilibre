import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthorListComponent } from './authors/components/author-list/author-list.component';
import { AuthorDetailsComponent } from './authors/components/author-details/author-details.component';
import { AuthorCreateComponent } from './authors/components/author-create/author-create.component';
import { AuthorUpdateComponent } from './authors/components/author-update/author-update.component';

const routes: Routes = [
  { path: '', redirectTo: '/authors', pathMatch: 'full' },
  { path: 'authors', component: AuthorListComponent },
  { path: 'authors/:id', component: AuthorDetailsComponent },
  { path: 'create', component: AuthorCreateComponent },
  { path: 'update/:id', component: AuthorUpdateComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
