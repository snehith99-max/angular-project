import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrTrnPurchaselegderComponent } from './pmr-trn-purchaselegder.component';

describe('PmrTrnPurchaselegderComponent', () => {
  let component: PmrTrnPurchaselegderComponent;
  let fixture: ComponentFixture<PmrTrnPurchaselegderComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrTrnPurchaselegderComponent]
    });
    fixture = TestBed.createComponent(PmrTrnPurchaselegderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
