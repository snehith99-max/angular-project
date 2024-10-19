import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsRptStocktransferReportComponent } from './ims-rpt-stocktransfer-report.component';

describe('ImsRptStocktransferReportComponent', () => {
  let component: ImsRptStocktransferReportComponent;
  let fixture: ComponentFixture<ImsRptStocktransferReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsRptStocktransferReportComponent]
    });
    fixture = TestBed.createComponent(ImsRptStocktransferReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
