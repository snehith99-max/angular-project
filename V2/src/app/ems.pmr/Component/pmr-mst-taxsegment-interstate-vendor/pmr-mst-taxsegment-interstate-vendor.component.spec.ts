import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrMstTaxsegmentInterstateVendorComponent } from './pmr-mst-taxsegment-interstate-vendor.component';

describe('PmrMstTaxsegmentInterstateVendorComponent', () => {
  let component: PmrMstTaxsegmentInterstateVendorComponent;
  let fixture: ComponentFixture<PmrMstTaxsegmentInterstateVendorComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrMstTaxsegmentInterstateVendorComponent]
    });
    fixture = TestBed.createComponent(PmrMstTaxsegmentInterstateVendorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
