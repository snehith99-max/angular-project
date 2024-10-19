import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PblTrnPaymentaddconfirmComponent } from './pbl-trn-paymentaddconfirm.component';

describe('PblTrnPaymentaddconfirmComponent', () => {
  let component: PblTrnPaymentaddconfirmComponent;
  let fixture: ComponentFixture<PblTrnPaymentaddconfirmComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PblTrnPaymentaddconfirmComponent]
    });
    fixture = TestBed.createComponent(PblTrnPaymentaddconfirmComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
