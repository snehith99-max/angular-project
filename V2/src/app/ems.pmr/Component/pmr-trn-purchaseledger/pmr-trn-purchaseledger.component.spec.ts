import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrTrnPurchaseledgerComponent } from './pmr-trn-purchaseledger.component';

describe('PmrTrnPurchaseledgerComponent', () => {
  let component: PmrTrnPurchaseledgerComponent;
  let fixture: ComponentFixture<PmrTrnPurchaseledgerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrTrnPurchaseledgerComponent]
    });
    fixture = TestBed.createComponent(PmrTrnPurchaseledgerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
