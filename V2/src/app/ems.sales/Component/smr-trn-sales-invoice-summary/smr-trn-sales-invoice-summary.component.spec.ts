import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnSalesInvoiceSummaryComponent } from './smr-trn-sales-invoice-summary.component';

describe('SmrTrnSalesInvoiceSummaryComponent', () => {
  let component: SmrTrnSalesInvoiceSummaryComponent;
  let fixture: ComponentFixture<SmrTrnSalesInvoiceSummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnSalesInvoiceSummaryComponent]
    });
    fixture = TestBed.createComponent(SmrTrnSalesInvoiceSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
