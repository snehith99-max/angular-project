import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PayRptEmployeepaymentdetailsviewComponent } from './pay-rpt-employeepaymentdetailsview.component';

describe('PayRptEmployeepaymentdetailsviewComponent', () => {
  let component: PayRptEmployeepaymentdetailsviewComponent;
  let fixture: ComponentFixture<PayRptEmployeepaymentdetailsviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PayRptEmployeepaymentdetailsviewComponent]
    });
    fixture = TestBed.createComponent(PayRptEmployeepaymentdetailsviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
