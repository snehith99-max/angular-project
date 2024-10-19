import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnPurchasereturnSummaryComponent } from './ims-trn-purchasereturn-summary.component';

describe('ImsTrnPurchasereturnSummaryComponent', () => {
  let component: ImsTrnPurchasereturnSummaryComponent;
  let fixture: ComponentFixture<ImsTrnPurchasereturnSummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnPurchasereturnSummaryComponent]
    });
    fixture = TestBed.createComponent(ImsTrnPurchasereturnSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
