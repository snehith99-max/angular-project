import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccRptTrialbalanceComponent } from './acc-rpt-trialbalance.component';

describe('AccRptTrialbalanceComponent', () => {
  let component: AccRptTrialbalanceComponent;
  let fixture: ComponentFixture<AccRptTrialbalanceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccRptTrialbalanceComponent]
    });
    fixture = TestBed.createComponent(AccRptTrialbalanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
