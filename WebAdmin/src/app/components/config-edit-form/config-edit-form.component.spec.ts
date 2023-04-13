import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfigEditFormComponent } from './config-edit-form.component';

describe('ConfigEditFormComponent', () => {
  let component: ConfigEditFormComponent;
  let fixture: ComponentFixture<ConfigEditFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ConfigEditFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfigEditFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
