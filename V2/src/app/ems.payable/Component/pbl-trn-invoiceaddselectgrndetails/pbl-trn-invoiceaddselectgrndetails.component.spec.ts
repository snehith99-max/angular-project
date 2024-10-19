import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PblTrnInvoiceaddselectgrndetailsComponent } from './pbl-trn-invoiceaddselectgrndetails.component';

describe('PblTrnInvoiceaddselectgrndetailsComponent', () => {
  let component: PblTrnInvoiceaddselectgrndetailsComponent;
  let fixture: ComponentFixture<PblTrnInvoiceaddselectgrndetailsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PblTrnInvoiceaddselectgrndetailsComponent]
    });
    fixture = TestBed.createComponent(PblTrnInvoiceaddselectgrndetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
