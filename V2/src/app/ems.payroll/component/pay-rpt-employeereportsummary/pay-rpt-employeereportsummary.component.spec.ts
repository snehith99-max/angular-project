import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PayRptEmployeereportsummaryComponent } from './pay-rpt-employeereportsummary.component';

describe('PayRptEmployeereportsummaryComponent', () => {
  let component: PayRptEmployeereportsummaryComponent;
  let fixture: ComponentFixture<PayRptEmployeereportsummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PayRptEmployeereportsummaryComponent]
    });
    fixture = TestBed.createComponent(PayRptEmployeereportsummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
