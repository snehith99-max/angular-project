import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccRptCreditorreportviewComponent } from './acc-rpt-creditorreportview.component';

describe('AccRptCreditorreportviewComponent', () => {
  let component: AccRptCreditorreportviewComponent;
  let fixture: ComponentFixture<AccRptCreditorreportviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccRptCreditorreportviewComponent]
    });
    fixture = TestBed.createComponent(AccRptCreditorreportviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
