import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsRptProductissueReportComponent } from './ims-rpt-productissue-report.component';

describe('ImsRptProductissueReportComponent', () => {
  let component: ImsRptProductissueReportComponent;
  let fixture: ComponentFixture<ImsRptProductissueReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsRptProductissueReportComponent]
    });
    fixture = TestBed.createComponent(ImsRptProductissueReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
