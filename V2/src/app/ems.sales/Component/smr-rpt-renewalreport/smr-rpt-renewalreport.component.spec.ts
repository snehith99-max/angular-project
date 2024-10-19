import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrRptRenewalreportComponent } from './smr-rpt-renewalreport.component';

describe('SmrRptRenewalreportComponent', () => {
  let component: SmrRptRenewalreportComponent;
  let fixture: ComponentFixture<SmrRptRenewalreportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrRptRenewalreportComponent]
    });
    fixture = TestBed.createComponent(SmrRptRenewalreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
