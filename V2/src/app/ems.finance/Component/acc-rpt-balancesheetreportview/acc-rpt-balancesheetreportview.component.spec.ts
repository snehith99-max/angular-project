import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccRptBalancesheetreportviewComponent } from './acc-rpt-balancesheetreportview.component';

describe('AccRptBalancesheetreportviewComponent', () => {
  let component: AccRptBalancesheetreportviewComponent;
  let fixture: ComponentFixture<AccRptBalancesheetreportviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccRptBalancesheetreportviewComponent]
    });
    fixture = TestBed.createComponent(AccRptBalancesheetreportviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
