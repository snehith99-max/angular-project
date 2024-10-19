import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrMstTaxsegmentUnassignVendorComponent } from './pmr-mst-taxsegment-unassign-vendor.component';

describe('PmrMstTaxsegmentUnassignVendorComponent', () => {
  let component: PmrMstTaxsegmentUnassignVendorComponent;
  let fixture: ComponentFixture<PmrMstTaxsegmentUnassignVendorComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrMstTaxsegmentUnassignVendorComponent]
    });
    fixture = TestBed.createComponent(PmrMstTaxsegmentUnassignVendorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
