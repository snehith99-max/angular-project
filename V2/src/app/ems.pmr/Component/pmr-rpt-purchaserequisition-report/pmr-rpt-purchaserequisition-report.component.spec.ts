import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrRptPurchaserequisitionReportComponent } from './pmr-rpt-purchaserequisition-report.component';

describe('PmrRptPurchaserequisitionReportComponent', () => {
  let component: PmrRptPurchaserequisitionReportComponent;
  let fixture: ComponentFixture<PmrRptPurchaserequisitionReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrRptPurchaserequisitionReportComponent]
    });
    fixture = TestBed.createComponent(PmrRptPurchaserequisitionReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
