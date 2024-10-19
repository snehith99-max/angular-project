import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnPurchaseHistoryviewComponent } from './ims-trn-purchase-historyview.component';

describe('ImsTrnPurchaseHistoryviewComponent', () => {
  let component: ImsTrnPurchaseHistoryviewComponent;
  let fixture: ComponentFixture<ImsTrnPurchaseHistoryviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnPurchaseHistoryviewComponent]
    });
    fixture = TestBed.createComponent(ImsTrnPurchaseHistoryviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
