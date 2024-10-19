import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnShopifyordersComponent } from './smr-trn-shopifyorders.component';

describe('SmrTrnShopifyordersComponent', () => {
  let component: SmrTrnShopifyordersComponent;
  let fixture: ComponentFixture<SmrTrnShopifyordersComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnShopifyordersComponent]
    });
    fixture = TestBed.createComponent(SmrTrnShopifyordersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
