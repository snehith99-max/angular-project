import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrRptPaymentorderReportComponent } from './pmr-rpt-paymentorder-report.component';

describe('PmrRptPaymentorderReportComponent', () => {
  let component: PmrRptPaymentorderReportComponent;
  let fixture: ComponentFixture<PmrRptPaymentorderReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrRptPaymentorderReportComponent]
    });
    fixture = TestBed.createComponent(PmrRptPaymentorderReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
