import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccTrnMultipleExpenses2singlepaymentComponent } from './acc-trn-multiple-expenses2singlepayment.component';

describe('AccTrnMultipleExpenses2singlepaymentComponent', () => {
  let component: AccTrnMultipleExpenses2singlepaymentComponent;
  let fixture: ComponentFixture<AccTrnMultipleExpenses2singlepaymentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccTrnMultipleExpenses2singlepaymentComponent]
    });
    fixture = TestBed.createComponent(AccTrnMultipleExpenses2singlepaymentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
