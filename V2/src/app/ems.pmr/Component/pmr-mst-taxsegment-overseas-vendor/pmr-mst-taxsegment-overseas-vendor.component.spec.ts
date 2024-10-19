import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrMstTaxsegmentOverseasVendorComponent } from './pmr-mst-taxsegment-overseas-vendor.component';

describe('PmrMstTaxsegmentOverseasVendorComponent', () => {
  let component: PmrMstTaxsegmentOverseasVendorComponent;
  let fixture: ComponentFixture<PmrMstTaxsegmentOverseasVendorComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrMstTaxsegmentOverseasVendorComponent]
    });
    fixture = TestBed.createComponent(PmrMstTaxsegmentOverseasVendorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
