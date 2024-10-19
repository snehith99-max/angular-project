import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnPurchasereturnAddComponent } from './ims-trn-purchasereturn-add.component';

describe('ImsTrnPurchasereturnAddComponent', () => {
  let component: ImsTrnPurchasereturnAddComponent;
  let fixture: ComponentFixture<ImsTrnPurchasereturnAddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnPurchasereturnAddComponent]
    });
    fixture = TestBed.createComponent(ImsTrnPurchasereturnAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
