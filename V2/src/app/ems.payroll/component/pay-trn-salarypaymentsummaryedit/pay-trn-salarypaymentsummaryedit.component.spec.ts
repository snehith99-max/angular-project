import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PayTrnSalarypaymentsummaryeditComponent } from './pay-trn-salarypaymentsummaryedit.component';

describe('PayTrnSalarypaymentsummaryeditComponent', () => {
  let component: PayTrnSalarypaymentsummaryeditComponent;
  let fixture: ComponentFixture<PayTrnSalarypaymentsummaryeditComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PayTrnSalarypaymentsummaryeditComponent]
    });
    fixture = TestBed.createComponent(PayTrnSalarypaymentsummaryeditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
