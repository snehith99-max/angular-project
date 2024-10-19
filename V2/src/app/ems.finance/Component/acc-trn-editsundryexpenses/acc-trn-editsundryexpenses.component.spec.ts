import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccTrnEditsundryexpensesComponent } from './acc-trn-editsundryexpenses.component';

describe('AccTrnEditsundryexpensesComponent', () => {
  let component: AccTrnEditsundryexpensesComponent;
  let fixture: ComponentFixture<AccTrnEditsundryexpensesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccTrnEditsundryexpensesComponent]
    });
    fixture = TestBed.createComponent(AccTrnEditsundryexpensesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
