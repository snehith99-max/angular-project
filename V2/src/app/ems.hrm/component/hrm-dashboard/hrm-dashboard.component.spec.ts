import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmDashboardComponent } from './hrm-dashboard.component';

describe('HrmDashboardComponent', () => {
  let component: HrmDashboardComponent;
  let fixture: ComponentFixture<HrmDashboardComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmDashboardComponent]
    });
    fixture = TestBed.createComponent(HrmDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
