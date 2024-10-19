import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsRptMaterialtrackerReportComponent } from './ims-rpt-materialtracker-report.component';

describe('ImsRptMaterialtrackerReportComponent', () => {
  let component: ImsRptMaterialtrackerReportComponent;
  let fixture: ComponentFixture<ImsRptMaterialtrackerReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsRptMaterialtrackerReportComponent]
    });
    fixture = TestBed.createComponent(ImsRptMaterialtrackerReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
