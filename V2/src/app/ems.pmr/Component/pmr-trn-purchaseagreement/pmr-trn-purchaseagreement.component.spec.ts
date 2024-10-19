import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrTrnPurchaseagreementComponent } from './pmr-trn-purchaseagreement.component';

describe('PmrTrnPurchaseagreementComponent', () => {
  let component: PmrTrnPurchaseagreementComponent;
  let fixture: ComponentFixture<PmrTrnPurchaseagreementComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrTrnPurchaseagreementComponent]
    });
    fixture = TestBed.createComponent(PmrTrnPurchaseagreementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
