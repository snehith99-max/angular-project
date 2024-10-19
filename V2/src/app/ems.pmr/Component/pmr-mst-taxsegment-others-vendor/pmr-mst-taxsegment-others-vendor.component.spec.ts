import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrMstTaxsegmentOthersVendorComponent } from './pmr-mst-taxsegment-others-vendor.component';

describe('PmrMstTaxsegmentOthersVendorComponent', () => {
  let component: PmrMstTaxsegmentOthersVendorComponent;
  let fixture: ComponentFixture<PmrMstTaxsegmentOthersVendorComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrMstTaxsegmentOthersVendorComponent]
    });
    fixture = TestBed.createComponent(PmrMstTaxsegmentOthersVendorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
