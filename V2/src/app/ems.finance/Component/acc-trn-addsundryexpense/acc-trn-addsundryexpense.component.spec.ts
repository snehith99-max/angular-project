import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccTrnAddsundryexpenseComponent } from './acc-trn-addsundryexpense.component';

describe('AccTrnAddsundryexpenseComponent', () => {
  let component: AccTrnAddsundryexpenseComponent;
  let fixture: ComponentFixture<AccTrnAddsundryexpenseComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccTrnAddsundryexpenseComponent]
    });
    fixture = TestBed.createComponent(AccTrnAddsundryexpenseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
