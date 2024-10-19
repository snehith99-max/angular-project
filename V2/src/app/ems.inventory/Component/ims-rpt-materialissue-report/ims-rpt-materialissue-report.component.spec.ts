import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsRptMaterialissueReportComponent } from './ims-rpt-materialissue-report.component';

describe('ImsRptMaterialissueReportComponent', () => {
  let component: ImsRptMaterialissueReportComponent;
  let fixture: ComponentFixture<ImsRptMaterialissueReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsRptMaterialissueReportComponent]
    });
    fixture = TestBed.createComponent(ImsRptMaterialissueReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
