import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnShopifyproductComponent } from './smr-trn-shopifyproduct.component';

describe('SmrTrnShopifyproductComponent', () => {
  let component: SmrTrnShopifyproductComponent;
  let fixture: ComponentFixture<SmrTrnShopifyproductComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnShopifyproductComponent]
    });
    fixture = TestBed.createComponent(SmrTrnShopifyproductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
