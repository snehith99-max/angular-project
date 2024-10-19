import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccRptDebtorDetailedreportComponent } from './acc-rpt-debtor-detailedreport.component';

describe('AccRptDebtorDetailedreportComponent', () => {
  let component: AccRptDebtorDetailedreportComponent;
  let fixture: ComponentFixture<AccRptDebtorDetailedreportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccRptDebtorDetailedreportComponent]
    });
    fixture = TestBed.createComponent(AccRptDebtorDetailedreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
