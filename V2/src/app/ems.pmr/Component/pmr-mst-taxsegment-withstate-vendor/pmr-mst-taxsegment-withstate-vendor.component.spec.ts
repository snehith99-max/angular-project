import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrMstTaxsegmentWithstateVendorComponent } from './pmr-mst-taxsegment-withstate-vendor.component';

describe('PmrMstTaxsegmentWithstateVendorComponent', () => {
  let component: PmrMstTaxsegmentWithstateVendorComponent;
  let fixture: ComponentFixture<PmrMstTaxsegmentWithstateVendorComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrMstTaxsegmentWithstateVendorComponent]
    });
    fixture = TestBed.createComponent(PmrMstTaxsegmentWithstateVendorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
