import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrMstTaxUnMap2ProductComponent } from './pmr-mst-tax-un-map2-product.component';

describe('PmrMstTaxUnMap2ProductComponent', () => {
  let component: PmrMstTaxUnMap2ProductComponent;
  let fixture: ComponentFixture<PmrMstTaxUnMap2ProductComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrMstTaxUnMap2ProductComponent]
    });
    fixture = TestBed.createComponent(PmrMstTaxUnMap2ProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
