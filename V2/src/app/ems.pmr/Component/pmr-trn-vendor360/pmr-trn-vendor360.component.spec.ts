import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrTrnVendor360Component } from './pmr-trn-vendor360.component';

describe('PmrTrnVendor360Component', () => {
  let component: PmrTrnVendor360Component;
  let fixture: ComponentFixture<PmrTrnVendor360Component>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrTrnVendor360Component]
    });
    fixture = TestBed.createComponent(PmrTrnVendor360Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
