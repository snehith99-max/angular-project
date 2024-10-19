import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrTrnPurchaseorderAddconfirmComponent } from './pmr-trn-purchaseorder-addconfirm.component';

describe('PmrTrnPurchaseorderAddconfirmComponent', () => {
  let component: PmrTrnPurchaseorderAddconfirmComponent;
  let fixture: ComponentFixture<PmrTrnPurchaseorderAddconfirmComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrTrnPurchaseorderAddconfirmComponent]
    });
    fixture = TestBed.createComponent(PmrTrnPurchaseorderAddconfirmComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
