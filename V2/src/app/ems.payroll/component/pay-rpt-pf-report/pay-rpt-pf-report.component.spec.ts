import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PayRptPfReportComponent } from './pay-rpt-pf-report.component';

describe('PayRptPfReportComponent', () => {
  let component: PayRptPfReportComponent;
  let fixture: ComponentFixture<PayRptPfReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PayRptPfReportComponent]
    });
    fixture = TestBed.createComponent(PayRptPfReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
