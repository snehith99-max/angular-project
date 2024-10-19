import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsRptStockstatusReportComponent } from './ims-rpt-stockstatus-report.component';

describe('ImsRptStockstatusReportComponent', () => {
  let component: ImsRptStockstatusReportComponent;
  let fixture: ComponentFixture<ImsRptStockstatusReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsRptStockstatusReportComponent]
    });
    fixture = TestBed.createComponent(ImsRptStockstatusReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
