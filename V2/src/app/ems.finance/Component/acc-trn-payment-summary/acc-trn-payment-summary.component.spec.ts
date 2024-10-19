import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccTrnPaymentSummaryComponent } from './acc-trn-payment-summary.component';

describe('AccTrnPaymentSummaryComponent', () => {
  let component: AccTrnPaymentSummaryComponent;
  let fixture: ComponentFixture<AccTrnPaymentSummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccTrnPaymentSummaryComponent]
    });
    fixture = TestBed.createComponent(AccTrnPaymentSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
