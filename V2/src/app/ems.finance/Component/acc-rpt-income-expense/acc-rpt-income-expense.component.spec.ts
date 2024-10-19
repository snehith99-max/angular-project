import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccRptIncomeExpenseComponent } from './acc-rpt-income-expense.component';

describe('AccRptIncomeExpenseComponent', () => {
  let component: AccRptIncomeExpenseComponent;
  let fixture: ComponentFixture<AccRptIncomeExpenseComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccRptIncomeExpenseComponent]
    });
    fixture = TestBed.createComponent(AccRptIncomeExpenseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
