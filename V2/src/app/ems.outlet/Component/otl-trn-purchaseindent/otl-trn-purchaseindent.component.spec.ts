import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlTrnPurchaseindentComponent } from './otl-trn-purchaseindent.component';

describe('OtlTrnPurchaseindentComponent', () => {
  let component: OtlTrnPurchaseindentComponent;
  let fixture: ComponentFixture<OtlTrnPurchaseindentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlTrnPurchaseindentComponent]
    });
    fixture = TestBed.createComponent(OtlTrnPurchaseindentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
