import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlTrnPurchaseindentviewComponent } from './otl-trn-purchaseindentview.component';

describe('OtlTrnPurchaseindentviewComponent', () => {
  let component: OtlTrnPurchaseindentviewComponent;
  let fixture: ComponentFixture<OtlTrnPurchaseindentviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlTrnPurchaseindentviewComponent]
    });
    fixture = TestBed.createComponent(OtlTrnPurchaseindentviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
