import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrMstTaxsegmentTotalVendorComponent } from './pmr-mst-taxsegment-total-vendor.component';

describe('PmrMstTaxsegmentTotalVendorComponent', () => {
  let component: PmrMstTaxsegmentTotalVendorComponent;
  let fixture: ComponentFixture<PmrMstTaxsegmentTotalVendorComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrMstTaxsegmentTotalVendorComponent]
    });
    fixture = TestBed.createComponent(PmrMstTaxsegmentTotalVendorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
