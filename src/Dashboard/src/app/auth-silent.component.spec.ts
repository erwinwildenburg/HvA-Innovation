import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthSilentComponent } from './auth-silent.component';

describe('AuthSilentComponent', () => {
  let component: AuthSilentComponent;
  let fixture: ComponentFixture<AuthSilentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AuthSilentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuthSilentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
