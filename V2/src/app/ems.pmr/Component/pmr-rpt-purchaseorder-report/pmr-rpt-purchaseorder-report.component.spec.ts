import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrRptPurchaseorderReportComponent } from './pmr-rpt-purchaseorder-report.component';

describe('PmrRptPurchaseorderReportComponent', () => {
  let component: PmrRptPurchaseorderReportComponent;
  let fixture: ComponentFixture<PmrRptPurchaseorderReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrRptPurchaseorderReportComponent]
    });
    fixture = TestBed.createComponent(PmrRptPurchaseorderReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
