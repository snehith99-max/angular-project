import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PblTrnInvoicePricechangingRequestComponent } from './pbl-trn-invoice-pricechanging-request.component';

describe('PblTrnInvoicePricechangingRequestComponent', () => {
  let component: PblTrnInvoicePricechangingRequestComponent;
  let fixture: ComponentFixture<PblTrnInvoicePricechangingRequestComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PblTrnInvoicePricechangingRequestComponent]
    });
    fixture = TestBed.createComponent(PblTrnInvoicePricechangingRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
