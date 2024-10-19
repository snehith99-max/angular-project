import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrRptAgreementreportComponent } from './pmr-rpt-agreementreport.component';

describe('PmrRptAgreementreportComponent', () => {
  let component: PmrRptAgreementreportComponent;
  let fixture: ComponentFixture<PmrRptAgreementreportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrRptAgreementreportComponent]
    });
    fixture = TestBed.createComponent(PmrRptAgreementreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
