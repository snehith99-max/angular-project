import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccTrnExpenseAddComponent } from './acc-trn-expense-add.component';

describe('AccTrnExpenseAddComponent', () => {
  let component: AccTrnExpenseAddComponent;
  let fixture: ComponentFixture<AccTrnExpenseAddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccTrnExpenseAddComponent]
    });
    fixture = TestBed.createComponent(AccTrnExpenseAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
