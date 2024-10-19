import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PfRptEsiReportComponent } from './pf-rpt-esi-report.component';

describe('PfRptEsiReportComponent', () => {
  let component: PfRptEsiReportComponent;
  let fixture: ComponentFixture<PfRptEsiReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PfRptEsiReportComponent]
    });
    fixture = TestBed.createComponent(PfRptEsiReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
