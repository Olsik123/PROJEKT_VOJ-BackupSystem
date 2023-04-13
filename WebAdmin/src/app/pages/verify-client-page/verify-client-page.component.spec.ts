import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VerifyClientPageComponent } from './verify-client-page.component';

describe('VerifyClientPageComponent', () => {
  let component: VerifyClientPageComponent;
  let fixture: ComponentFixture<VerifyClientPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VerifyClientPageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VerifyClientPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
