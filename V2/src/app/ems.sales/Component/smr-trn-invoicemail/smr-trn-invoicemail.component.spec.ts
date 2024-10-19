import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnInvoicemailComponent } from './smr-trn-invoicemail.component';

describe('SmrTrnInvoicemailComponent', () => {
  let component: SmrTrnInvoicemailComponent;
  let fixture: ComponentFixture<SmrTrnInvoicemailComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnInvoicemailComponent]
    });
    fixture = TestBed.createComponent(SmrTrnInvoicemailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
