import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccTrnSundryexpenseComponent } from './acc-trn-sundryexpense.component';

describe('AccTrnSundryexpenseComponent', () => {
  let component: AccTrnSundryexpenseComponent;
  let fixture: ComponentFixture<AccTrnSundryexpenseComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccTrnSundryexpenseComponent]
    });
    fixture = TestBed.createComponent(AccTrnSundryexpenseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
