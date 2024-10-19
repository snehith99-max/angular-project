import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnRaisePurchaseorderComponent } from './smr-trn-raise-purchaseorder.component';

describe('SmrTrnRaisePurchaseorderComponent', () => {
  let component: SmrTrnRaisePurchaseorderComponent;
  let fixture: ComponentFixture<SmrTrnRaisePurchaseorderComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnRaisePurchaseorderComponent]
    });
    fixture = TestBed.createComponent(SmrTrnRaisePurchaseorderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
