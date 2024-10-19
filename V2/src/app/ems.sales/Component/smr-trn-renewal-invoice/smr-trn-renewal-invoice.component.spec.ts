import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnRenewalInvoiceComponent } from './smr-trn-renewal-invoice.component';

describe('SmrTrnRenewalInvoiceComponent', () => {
  let component: SmrTrnRenewalInvoiceComponent;
  let fixture: ComponentFixture<SmrTrnRenewalInvoiceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnRenewalInvoiceComponent]
    });
    fixture = TestBed.createComponent(SmrTrnRenewalInvoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
