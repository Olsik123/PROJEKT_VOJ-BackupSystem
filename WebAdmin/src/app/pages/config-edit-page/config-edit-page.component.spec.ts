import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfigEditPageComponent } from './config-edit-page.component';

describe('ConfigEditPageComponent', () => {
  let component: ConfigEditPageComponent;
  let fixture: ComponentFixture<ConfigEditPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ConfigEditPageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfigEditPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
