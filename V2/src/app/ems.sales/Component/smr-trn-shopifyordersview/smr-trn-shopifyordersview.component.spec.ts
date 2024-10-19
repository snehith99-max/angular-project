import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnShopifyordersviewComponent } from './smr-trn-shopifyordersview.component';

describe('SmrTrnShopifyordersviewComponent', () => {
  let component: SmrTrnShopifyordersviewComponent;
  let fixture: ComponentFixture<SmrTrnShopifyordersviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnShopifyordersviewComponent]
    });
    fixture = TestBed.createComponent(SmrTrnShopifyordersviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
