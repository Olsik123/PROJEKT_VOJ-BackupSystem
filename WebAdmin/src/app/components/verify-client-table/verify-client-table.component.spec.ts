import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VerifyClientTableComponent } from './verify-client-table.component';

describe('VerifyClientTableComponent', () => {
  let component: VerifyClientTableComponent;
  let fixture: ComponentFixture<VerifyClientTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VerifyClientTableComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VerifyClientTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
