import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccRptVendor360Component } from './acc-rpt-vendor360.component';

describe('AccRptVendor360Component', () => {
  let component: AccRptVendor360Component;
  let fixture: ComponentFixture<AccRptVendor360Component>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccRptVendor360Component]
    });
    fixture = TestBed.createComponent(AccRptVendor360Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
