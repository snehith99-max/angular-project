import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LglDashboardComponent } from './lgl-dashboard.component';

describe('LglDashboardComponent', () => {
  let component: LglDashboardComponent;
  let fixture: ComponentFixture<LglDashboardComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LglDashboardComponent]
    });
    fixture = TestBed.createComponent(LglDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
