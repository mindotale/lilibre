import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms'; 
import { Author } from '../../models/author.model'
import { AuthorService } from '../../services/author.service'

@Component({
  selector: 'app-author-create',
  templateUrl: './author-create.component.html',
  styleUrls: ['./author-create.component.css']
})
export class AuthorCreateComponent {
  authorForm: FormGroup; // Declare a FormGroup

  constructor(
    private authorService: AuthorService,
    private router: Router,
    private fb: FormBuilder // Inject FormBuilder
  ) {
    this.authorForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(100)]],
      description: ['', [Validators.required]],
      birthYear: ['', [Validators.required, Validators.min(1000), Validators.max(new Date().getFullYear())]]
    });
  }

  createAuthor(): void {
    // Check if the form is valid before submitting
    if (this.authorForm.valid) {
      const author: Author = {
        id: 0,
        name: this.authorForm.value.name,
        description: this.authorForm.value.description,
        birthYear: this.authorForm.value.birthYear
      };

      this.authorService.createAuthor(author)
        .subscribe(() => {
          this.router.navigate(['/authors']);
        });
    }
  }
}
