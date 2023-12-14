import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Author } from '../../models/author.model'
import { AuthorService } from '../../services/author.service'

@Component({
  selector: 'app-author-list',
  templateUrl: './author-list.component.html',
  styleUrls: ['./author-list.component.css']
})
export class AuthorListComponent implements OnInit {
  authors!: Author[];

  constructor(private authorService: AuthorService, private router: Router) { }

  ngOnInit(): void {
    this.getAuthors();
  }

  getAuthors(): void {
    this.authorService.getAuthors()
      .subscribe(authors => {
        this.authors = authors;
      });
  }

  viewAuthorDetails(id: number): void {
    this.router.navigate(['/authors', id]);
  }

  deleteAuthor(id: number): void {
    this.authorService.deleteAuthor(id)
      .subscribe(() => {
        this.getAuthors(); // Refresh the list after deletion
      });
  }
}
