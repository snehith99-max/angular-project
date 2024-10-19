import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrTrnPurchaseorderEditComponent } from './pmr-trn-purchaseorder-edit.component';

describe('PmrTrnPurchaseorderEditComponent', () => {
  let component: PmrTrnPurchaseorderEditComponent;
  let fixture: ComponentFixture<PmrTrnPurchaseorderEditComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrTrnPurchaseorderEditComponent]
    });
    fixture = TestBed.createComponent(PmrTrnPurchaseorderEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
