import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccTrnAddPaymentComponent } from './acc-trn-add-payment.component';

describe('AccTrnAddPaymentComponent', () => {
  let component: AccTrnAddPaymentComponent;
  let fixture: ComponentFixture<AccTrnAddPaymentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccTrnAddPaymentComponent]
    });
    fixture = TestBed.createComponent(AccTrnAddPaymentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
