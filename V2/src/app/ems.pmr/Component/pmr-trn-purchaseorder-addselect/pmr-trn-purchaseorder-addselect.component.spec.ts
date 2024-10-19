import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrTrnPurchaseorderAddselectComponent } from './pmr-trn-purchaseorder-addselect.component';

describe('PmrTrnPurchaseorderAddselectComponent', () => {
  let component: PmrTrnPurchaseorderAddselectComponent;
  let fixture: ComponentFixture<PmrTrnPurchaseorderAddselectComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrTrnPurchaseorderAddselectComponent]
    });
    fixture = TestBed.createComponent(PmrTrnPurchaseorderAddselectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
