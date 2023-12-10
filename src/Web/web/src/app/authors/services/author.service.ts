import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Author } from '../models/author.model';

@Injectable({
  providedIn: 'root'
})
export class AuthorService {
  private apiUrl = 'http://localhost:5000';

  constructor(private http: HttpClient) { }

  getAuthors(): Observable<Author[]> {
    return this.http.get<Author[]>(`${this.apiUrl}/api/v1/authors`);
  }

  getAuthorById(id: number): Observable<Author> {
    return this.http.get<Author>(`${this.apiUrl}/api/v1/authors/${id}`);
  }

  createAuthor(author: Author): Observable<Author> {
    return this.http.post<Author>(`${this.apiUrl}/api/v1/authors`, author);
  }

  updateAuthor(id: number, author: Author): Observable<any> {
    return this.http.put(`${this.apiUrl}/api/v1/authors/${id}`, author);
  }

  deleteAuthor(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/api/v1/authors/${id}`);
  }
}
