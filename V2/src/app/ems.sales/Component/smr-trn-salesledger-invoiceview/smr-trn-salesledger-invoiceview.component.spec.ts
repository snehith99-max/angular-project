import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnSalesledgerInvoiceviewComponent } from './smr-trn-salesledger-invoiceview.component';

describe('SmrTrnSalesledgerInvoiceviewComponent', () => {
  let component: SmrTrnSalesledgerInvoiceviewComponent;
  let fixture: ComponentFixture<SmrTrnSalesledgerInvoiceviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnSalesledgerInvoiceviewComponent]
    });
    fixture = TestBed.createComponent(SmrTrnSalesledgerInvoiceviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
