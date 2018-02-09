import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfileOutputComponent } from './profile-output.component';

describe('ProfileOutputComponent', () => {
  let component: ProfileOutputComponent;
  let fixture: ComponentFixture<ProfileOutputComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProfileOutputComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProfileOutputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
