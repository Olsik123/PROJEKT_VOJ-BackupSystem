import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfigNewFormComponent } from './config-new-form.component';

describe('ConfigNewFormComponent', () => {
  let component: ConfigNewFormComponent;
  let fixture: ComponentFixture<ConfigNewFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ConfigNewFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfigNewFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
