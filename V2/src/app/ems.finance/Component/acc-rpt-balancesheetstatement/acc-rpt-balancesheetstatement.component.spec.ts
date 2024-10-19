import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccRptBalancesheetstatementComponent } from './acc-rpt-balancesheetstatement.component';

describe('AccRptBalancesheetstatementComponent', () => {
  let component: AccRptBalancesheetstatementComponent;
  let fixture: ComponentFixture<AccRptBalancesheetstatementComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccRptBalancesheetstatementComponent]
    });
    fixture = TestBed.createComponent(AccRptBalancesheetstatementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
