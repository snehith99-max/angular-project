import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnInvoiceReceiptComponent } from './smr-trn-invoice-receipt.component';

describe('SmrTrnInvoiceReceiptComponent', () => {
  let component: SmrTrnInvoiceReceiptComponent;
  let fixture: ComponentFixture<SmrTrnInvoiceReceiptComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnInvoiceReceiptComponent]
    });
    fixture = TestBed.createComponent(SmrTrnInvoiceReceiptComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
