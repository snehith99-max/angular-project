import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccRptLedgerliabilityreportComponent } from './acc-rpt-ledgerliabilityreport.component';

describe('AccRptLedgerliabilityreportComponent', () => {
  let component: AccRptLedgerliabilityreportComponent;
  let fixture: ComponentFixture<AccRptLedgerliabilityreportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccRptLedgerliabilityreportComponent]
    });
    fixture = TestBed.createComponent(AccRptLedgerliabilityreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
