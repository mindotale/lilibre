import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthorUpdateComponent } from './author-update.component';

describe('AuthorUpdateComponent', () => {
  let component: AuthorUpdateComponent;
  let fixture: ComponentFixture<AuthorUpdateComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AuthorUpdateComponent]
    });
    fixture = TestBed.createComponent(AuthorUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
