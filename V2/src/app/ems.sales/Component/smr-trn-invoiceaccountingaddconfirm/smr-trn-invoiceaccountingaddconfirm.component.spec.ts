import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnInvoiceaccountingaddconfirmComponent } from './smr-trn-invoiceaccountingaddconfirm.component';

describe('SmrTrnInvoiceaccountingaddconfirmComponent', () => {
  let component: SmrTrnInvoiceaccountingaddconfirmComponent;
  let fixture: ComponentFixture<SmrTrnInvoiceaccountingaddconfirmComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnInvoiceaccountingaddconfirmComponent]
    });
    fixture = TestBed.createComponent(SmrTrnInvoiceaccountingaddconfirmComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
