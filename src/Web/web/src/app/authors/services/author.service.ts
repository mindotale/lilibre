import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Author } from '../models/author.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthorService {
  private apiUrl = environment.apiUrl;
  private apiVersion = 'v1';
  private authorEndpoint = 'authors';

  constructor(private http: HttpClient) { }

  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'Unknown error occurred';
    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else {
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    console.error(errorMessage);
    return throwError(errorMessage);
  }

  getAuthors(): Observable<Author[]> {
    const url = `${this.apiUrl}/api/${this.apiVersion}/${this.authorEndpoint}`;
    return this.http.get<Author[]>(url).pipe(
      catchError(this.handleError)
    );
  }

  getAuthorById(id: number): Observable<Author> {
    const url = `${this.apiUrl}/api/${this.apiVersion}/${this.authorEndpoint}/${id}`;
    return this.http.get<Author>(url).pipe(
      catchError(this.handleError)
    );
  }

  createAuthor(author: Author): Observable<Author> {
    const url = `${this.apiUrl}/api/${this.apiVersion}/${this.authorEndpoint}`;
    return this.http.post<Author>(url, author).pipe(
      catchError(this.handleError)
    );
  }

  updateAuthor(id: number, author: Author): Observable<any> {
    const url = `${this.apiUrl}/api/${this.apiVersion}/${this.authorEndpoint}/${id}`;
    return this.http.put(url, author).pipe(
      catchError(this.handleError)
    );
  }

  deleteAuthor(id: number): Observable<any> {
    const url = `${this.apiUrl}/api/${this.apiVersion}/${this.authorEndpoint}/${id}`;
    return this.http.delete(url).pipe(
      catchError(this.handleError)
    );
  }
}
