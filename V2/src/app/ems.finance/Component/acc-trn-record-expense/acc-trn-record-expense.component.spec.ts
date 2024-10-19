import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccTrnRecordExpenseComponent } from './acc-trn-record-expense.component';

describe('AccTrnRecordExpenseComponent', () => {
  let component: AccTrnRecordExpenseComponent;
  let fixture: ComponentFixture<AccTrnRecordExpenseComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccTrnRecordExpenseComponent]
    });
    fixture = TestBed.createComponent(AccTrnRecordExpenseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
