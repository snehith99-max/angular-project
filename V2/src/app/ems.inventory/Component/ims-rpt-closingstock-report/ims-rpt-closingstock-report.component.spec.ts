import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsRptClosingstockReportComponent } from './ims-rpt-closingstock-report.component';

describe('ImsRptClosingstockReportComponent', () => {
  let component: ImsRptClosingstockReportComponent;
  let fixture: ComponentFixture<ImsRptClosingstockReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsRptClosingstockReportComponent]
    });
    fixture = TestBed.createComponent(ImsRptClosingstockReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
