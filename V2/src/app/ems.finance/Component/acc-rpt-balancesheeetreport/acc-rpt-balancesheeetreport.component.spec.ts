import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccRptBalancesheeetreportComponent } from './acc-rpt-balancesheeetreport.component';

describe('AccRptBalancesheeetreportComponent', () => {
  let component: AccRptBalancesheeetreportComponent;
  let fixture: ComponentFixture<AccRptBalancesheeetreportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccRptBalancesheeetreportComponent]
    });
    fixture = TestBed.createComponent(AccRptBalancesheeetreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
