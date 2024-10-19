import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrTrnAgreementtoInvoiceComponent } from './pmr-trn-agreementto-invoice.component';

describe('PmrTrnAgreementtoInvoiceComponent', () => {
  let component: PmrTrnAgreementtoInvoiceComponent;
  let fixture: ComponentFixture<PmrTrnAgreementtoInvoiceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrTrnAgreementtoInvoiceComponent]
    });
    fixture = TestBed.createComponent(PmrTrnAgreementtoInvoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
