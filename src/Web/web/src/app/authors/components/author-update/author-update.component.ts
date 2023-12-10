import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Author } from '../../models/author.model';
import { AuthorService } from '../../services/author.service';

@Component({
  selector: 'app-author-update',
  templateUrl: './author-update.component.html',
  styleUrls: ['./author-update.component.css']
})
export class AuthorUpdateComponent {
  authorForm: FormGroup;
  authorId: number;

  constructor(
    private authorService: AuthorService,
    private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder
  ) {
    this.authorId = +this.route.snapshot.paramMap.get('id')!;
    this.authorForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(100)]],
      description: ['', [Validators.required]],
      birthYear: ['', [Validators.required, Validators.min(1000), Validators.max(new Date().getFullYear())]]
    });

    // Fetch author details and set form values
    this.authorService.getAuthorById(this.authorId)
      .subscribe(author => {
        this.authorForm.patchValue({
          name: author.name,
          description: author.description,
          birthYear: author.birthYear
        });
      });
  }

  updateAuthor(): void {
    if (this.authorForm.valid) {
      const author: Author = {
        id: this.authorId,
        name: this.authorForm.value.name,
        description: this.authorForm.value.description,
        birthYear: this.authorForm.value.birthYear
      };

      this.authorService.updateAuthor(this.authorId, author)
        .subscribe(() => {
          this.router.navigate(['/authors']);
        });
    }
  }
}
