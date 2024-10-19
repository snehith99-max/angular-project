import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccRptIncomeandexpensestatementComponent } from './acc-rpt-incomeandexpensestatement.component';

describe('AccRptIncomeandexpensestatementComponent', () => {
  let component: AccRptIncomeandexpensestatementComponent;
  let fixture: ComponentFixture<AccRptIncomeandexpensestatementComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccRptIncomeandexpensestatementComponent]
    });
    fixture = TestBed.createComponent(AccRptIncomeandexpensestatementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
