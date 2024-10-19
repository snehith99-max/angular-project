import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnPurchasereturnAddselectComponent } from './ims-trn-purchasereturn-addselect.component';

describe('ImsTrnPurchasereturnAddselectComponent', () => {
  let component: ImsTrnPurchasereturnAddselectComponent;
  let fixture: ComponentFixture<ImsTrnPurchasereturnAddselectComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnPurchasereturnAddselectComponent]
    });
    fixture = TestBed.createComponent(ImsTrnPurchasereturnAddselectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
