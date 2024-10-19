import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnInvoiceaddComponent } from './smr-trn-invoiceadd.component';

describe('SmrTrnInvoiceaddComponent', () => {
  let component: SmrTrnInvoiceaddComponent;
  let fixture: ComponentFixture<SmrTrnInvoiceaddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnInvoiceaddComponent]
    });
    fixture = TestBed.createComponent(SmrTrnInvoiceaddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
