import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Author } from '../../models/author.model'
import { AuthorService } from '../../services/author.service'

@Component({
  selector: 'app-author-create',
  templateUrl: './author-create.component.html',
  styleUrls: ['./author-create.component.css']
})
export class AuthorCreateComponent {
  author: Author = {
    id: 0,
    name: '',
    description: '',
    birthYear: 0
  };

  constructor(private authorService: AuthorService, private router: Router) { }

  createAuthor(): void {
    this.authorService.createAuthor(this.author)
      .subscribe(() => {
        this.router.navigate(['/authors']); // Redirect to the list after creation
      });
  }
}
