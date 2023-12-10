import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthorListComponent } from './authors/components/author-list/author-list.component';
import { AuthorDetailsComponent } from './authors/components/author-details/author-details.component';
import { AuthorCreateComponent } from './authors/components/author-create/author-create.component';
import { AuthorUpdateComponent } from './authors/components/author-update/author-update.component';

@NgModule({
  declarations: [
    AppComponent,
    AuthorListComponent,
    AuthorDetailsComponent,
    AuthorCreateComponent,
    AuthorUpdateComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
