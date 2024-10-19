import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccRptLedgerreportComponent } from './acc-rpt-ledgerreport.component';

describe('AccRptLedgerreportComponent', () => {
  let component: AccRptLedgerreportComponent;
  let fixture: ComponentFixture<AccRptLedgerreportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccRptLedgerreportComponent]
    });
    fixture = TestBed.createComponent(AccRptLedgerreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
