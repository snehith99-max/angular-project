import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrRptReceiptreportComponent } from './smr-rpt-receiptreport.component';

describe('SmrRptReceiptreportComponent', () => {
  let component: SmrRptReceiptreportComponent;
  let fixture: ComponentFixture<SmrRptReceiptreportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrRptReceiptreportComponent]
    });
    fixture = TestBed.createComponent(SmrRptReceiptreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
