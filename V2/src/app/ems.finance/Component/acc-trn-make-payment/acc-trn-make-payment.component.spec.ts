import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccTrnMakePaymentComponent } from './acc-trn-make-payment.component';

describe('AccTrnMakePaymentComponent', () => {
  let component: AccTrnMakePaymentComponent;
  let fixture: ComponentFixture<AccTrnMakePaymentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccTrnMakePaymentComponent]
    });
    fixture = TestBed.createComponent(AccTrnMakePaymentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
