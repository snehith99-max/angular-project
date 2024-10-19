import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnPurchasehistoryComponent } from './ims-trn-purchasehistory.component';

describe('ImsTrnPurchasehistoryComponent', () => {
  let component: ImsTrnPurchasehistoryComponent;
  let fixture: ComponentFixture<ImsTrnPurchasehistoryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnPurchasehistoryComponent]
    });
    fixture = TestBed.createComponent(ImsTrnPurchasehistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
