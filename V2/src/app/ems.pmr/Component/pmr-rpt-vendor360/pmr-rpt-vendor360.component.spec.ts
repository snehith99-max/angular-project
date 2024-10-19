import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrRptVendor360Component } from './pmr-rpt-vendor360.component';

describe('PmrRptVendor360Component', () => {
  let component: PmrRptVendor360Component;
  let fixture: ComponentFixture<PmrRptVendor360Component>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrRptVendor360Component]
    });
    fixture = TestBed.createComponent(PmrRptVendor360Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
