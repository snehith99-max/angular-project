import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrTrnPurchaseagreementviewComponent } from './pmr-trn-purchaseagreementview.component';

describe('PmrTrnPurchaseagreementviewComponent', () => {
  let component: PmrTrnPurchaseagreementviewComponent;
  let fixture: ComponentFixture<PmrTrnPurchaseagreementviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrTrnPurchaseagreementviewComponent]
    });
    fixture = TestBed.createComponent(PmrTrnPurchaseagreementviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
