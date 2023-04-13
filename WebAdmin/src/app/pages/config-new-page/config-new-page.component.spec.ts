import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfigNewPageComponent } from './config-new-page.component';

describe('ConfigNewPageComponent', () => {
  let component: ConfigNewPageComponent;
  let fixture: ComponentFixture<ConfigNewPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ConfigNewPageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfigNewPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
