import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccRptLedgerincomereportComponent } from './acc-rpt-ledgerincomereport.component';

describe('AccRptLedgerincomereportComponent', () => {
  let component: AccRptLedgerincomereportComponent;
  let fixture: ComponentFixture<AccRptLedgerincomereportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccRptLedgerincomereportComponent]
    });
    fixture = TestBed.createComponent(AccRptLedgerincomereportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
