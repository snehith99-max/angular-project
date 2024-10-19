import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrRptOuststandingamountReportComponent } from './smr-rpt-ouststandingamount-report.component';

describe('SmrRptOuststandingamountReportComponent', () => {
  let component: SmrRptOuststandingamountReportComponent;
  let fixture: ComponentFixture<SmrRptOuststandingamountReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrRptOuststandingamountReportComponent]
    });
    fixture = TestBed.createComponent(SmrRptOuststandingamountReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
