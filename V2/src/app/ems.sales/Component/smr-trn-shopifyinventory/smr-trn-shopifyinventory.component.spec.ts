import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnShopifyinventoryComponent } from './smr-trn-shopifyinventory.component';

describe('SmrTrnShopifyinventoryComponent', () => {
  let component: SmrTrnShopifyinventoryComponent;
  let fixture: ComponentFixture<SmrTrnShopifyinventoryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnShopifyinventoryComponent]
    });
    fixture = TestBed.createComponent(SmrTrnShopifyinventoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
