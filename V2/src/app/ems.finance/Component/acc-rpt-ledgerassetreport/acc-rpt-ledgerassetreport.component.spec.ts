import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccRptLedgerassetreportComponent } from './acc-rpt-ledgerassetreport.component';

describe('AccRptLedgerassetreportComponent', () => {
  let component: AccRptLedgerassetreportComponent;
  let fixture: ComponentFixture<AccRptLedgerassetreportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccRptLedgerassetreportComponent]
    });
    fixture = TestBed.createComponent(AccRptLedgerassetreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
