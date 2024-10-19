import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrTrnLedgerInvoiceviewComponent } from './pmr-trn-ledger-invoiceview.component';

describe('PmrTrnLedgerInvoiceviewComponent', () => {
  let component: PmrTrnLedgerInvoiceviewComponent;
  let fixture: ComponentFixture<PmrTrnLedgerInvoiceviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrTrnLedgerInvoiceviewComponent]
    });
    fixture = TestBed.createComponent(PmrTrnLedgerInvoiceviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
