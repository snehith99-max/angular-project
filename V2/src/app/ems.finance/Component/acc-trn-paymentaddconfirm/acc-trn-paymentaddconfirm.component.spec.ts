import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccTrnPaymentaddconfirmComponent } from './acc-trn-paymentaddconfirm.component';

describe('AccTrnPaymentaddconfirmComponent', () => {
  let component: AccTrnPaymentaddconfirmComponent;
  let fixture: ComponentFixture<AccTrnPaymentaddconfirmComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccTrnPaymentaddconfirmComponent]
    });
    fixture = TestBed.createComponent(AccTrnPaymentaddconfirmComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
