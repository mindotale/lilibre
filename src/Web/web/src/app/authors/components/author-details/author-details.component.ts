import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Author } from '../../models/author.model';
import { AuthorService } from '../../services/author.service';

@Component({
  selector: 'app-author-details',
  templateUrl: './author-details.component.html',
  styleUrls: ['./author-details.component.css']
})
export class AuthorDetailsComponent implements OnInit {
  author!: Author;

  constructor(private route: ActivatedRoute, private router: Router, private authorService: AuthorService) { }

  ngOnInit(): void {
    const id = +this.route.snapshot.paramMap.get('id')!;
    this.authorService.getAuthorById(id)
      .subscribe((data) => {
        this.author = data;
      });
  }

  editAuthor(id: number): void {
    this.router.navigate(['/update', id]); // Redirect to update form
  }
}
