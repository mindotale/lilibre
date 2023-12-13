import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ReactiveFormsModule} from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthorListComponent } from './authors/components/author-list/author-list.component';
import { AuthorDetailsComponent } from './authors/components/author-details/author-details.component';
import { AuthorCreateComponent } from './authors/components/author-create/author-create.component';
import { AuthorUpdateComponent } from './authors/components/author-update/author-update.component';
import { LoggingInterceptor } from './interceptors/logging.interceptor';

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
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LoggingInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
