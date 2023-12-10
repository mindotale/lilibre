import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Author } from '../../models/author.model'
import { AuthorService } from '../../services/author.service'

@Component({
  selector: 'app-author-update',
  templateUrl: './author-update.component.html',
  styleUrls: ['./author-update.component.css']
})
export class AuthorUpdateComponent implements OnInit {
  id!: number;
  author!: Author;

  constructor(private route: ActivatedRoute, private authorService: AuthorService, private router: Router) { }

  ngOnInit(): void {
    this.id = this.route.snapshot.params['id'];
    this.authorService.getAuthorById(this.id)
      .subscribe(data => {
        this.author = data;
      });
  }

  updateAuthor(): void {
    this.authorService.updateAuthor(this.id, this.author)
      .subscribe(() => {
        this.router.navigate(['/authors']); // Redirect to the list after update
      });
  }
}
