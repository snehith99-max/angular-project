import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnPurchasereturnViewComponent } from './ims-trn-purchasereturn-view.component';

describe('ImsTrnPurchasereturnViewComponent', () => {
  let component: ImsTrnPurchasereturnViewComponent;
  let fixture: ComponentFixture<ImsTrnPurchasereturnViewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnPurchasereturnViewComponent]
    });
    fixture = TestBed.createComponent(ImsTrnPurchasereturnViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
