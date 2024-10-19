import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PblTrnInvoicepricechangeComponent } from './pbl-trn-invoicepricechange.component';

describe('PblTrnInvoicepricechangeComponent', () => {
  let component: PblTrnInvoicepricechangeComponent;
  let fixture: ComponentFixture<PblTrnInvoicepricechangeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PblTrnInvoicepricechangeComponent]
    });
    fixture = TestBed.createComponent(PblTrnInvoicepricechangeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
