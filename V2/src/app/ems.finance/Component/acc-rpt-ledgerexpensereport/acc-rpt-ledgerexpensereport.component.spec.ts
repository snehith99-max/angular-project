import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccRptLedgerexpensereportComponent } from './acc-rpt-ledgerexpensereport.component';

describe('AccRptLedgerexpensereportComponent', () => {
  let component: AccRptLedgerexpensereportComponent;
  let fixture: ComponentFixture<AccRptLedgerexpensereportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccRptLedgerexpensereportComponent]
    });
    fixture = TestBed.createComponent(AccRptLedgerexpensereportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
