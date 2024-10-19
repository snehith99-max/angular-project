import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnInvoiceviewComponent } from './smr-trn-invoiceview.component';

describe('SmrTrnInvoiceviewComponent', () => {
  let component: SmrTrnInvoiceviewComponent;
  let fixture: ComponentFixture<SmrTrnInvoiceviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnInvoiceviewComponent]
    });
    fixture = TestBed.createComponent(SmrTrnInvoiceviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
