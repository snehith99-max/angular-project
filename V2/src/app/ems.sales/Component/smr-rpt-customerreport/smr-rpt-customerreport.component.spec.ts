import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrRptCustomerreportComponent } from './smr-rpt-customerreport.component';

describe('SmrRptCustomerreportComponent', () => {
  let component: SmrRptCustomerreportComponent;
  let fixture: ComponentFixture<SmrRptCustomerreportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrRptCustomerreportComponent]
    });
    fixture = TestBed.createComponent(SmrRptCustomerreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
