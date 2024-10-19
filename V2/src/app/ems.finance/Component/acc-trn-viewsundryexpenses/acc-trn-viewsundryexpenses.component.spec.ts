import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccTrnViewsundryexpensesComponent } from './acc-trn-viewsundryexpenses.component';

describe('AccTrnViewsundryexpensesComponent', () => {
  let component: AccTrnViewsundryexpensesComponent;
  let fixture: ComponentFixture<AccTrnViewsundryexpensesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccTrnViewsundryexpensesComponent]
    });
    fixture = TestBed.createComponent(AccTrnViewsundryexpensesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
