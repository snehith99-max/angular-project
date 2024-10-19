import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnShopifypaymentComponent } from './smr-trn-shopifypayment.component';

describe('SmrTrnShopifypaymentComponent', () => {
  let component: SmrTrnShopifypaymentComponent;
  let fixture: ComponentFixture<SmrTrnShopifypaymentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnShopifypaymentComponent]
    });
    fixture = TestBed.createComponent(SmrTrnShopifypaymentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
