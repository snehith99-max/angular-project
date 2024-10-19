import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrRptPurchaseInvoiceReportComponent } from './pmr-rpt-purchase-invoice-report.component';

describe('PmrRptPurchaseInvoiceReportComponent', () => {
  let component: PmrRptPurchaseInvoiceReportComponent;
  let fixture: ComponentFixture<PmrRptPurchaseInvoiceReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrRptPurchaseInvoiceReportComponent]
    });
    fixture = TestBed.createComponent(PmrRptPurchaseInvoiceReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
