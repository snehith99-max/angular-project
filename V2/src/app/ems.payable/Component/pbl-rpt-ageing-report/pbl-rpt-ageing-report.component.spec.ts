import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PblRptAgeingReportComponent } from './pbl-rpt-ageing-report.component';

describe('PblRptAgeingReportComponent', () => {
  let component: PblRptAgeingReportComponent;
  let fixture: ComponentFixture<PblRptAgeingReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PblRptAgeingReportComponent]
    });
    fixture = TestBed.createComponent(PblRptAgeingReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
