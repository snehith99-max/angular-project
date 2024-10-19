import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnRenewaltoInvoiceComponent } from './smr-trn-renewalto-invoice.component';

describe('SmrTrnRenewaltoInvoiceComponent', () => {
  let component: SmrTrnRenewaltoInvoiceComponent;
  let fixture: ComponentFixture<SmrTrnRenewaltoInvoiceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnRenewaltoInvoiceComponent]
    });
    fixture = TestBed.createComponent(SmrTrnRenewaltoInvoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
