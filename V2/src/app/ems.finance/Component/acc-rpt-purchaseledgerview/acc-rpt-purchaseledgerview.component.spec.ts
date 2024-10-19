import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccRptPurchaseledgerviewComponent } from './acc-rpt-purchaseledgerview.component';

describe('AccRptPurchaseledgerviewComponent', () => {
  let component: AccRptPurchaseledgerviewComponent;
  let fixture: ComponentFixture<AccRptPurchaseledgerviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccRptPurchaseledgerviewComponent]
    });
    fixture = TestBed.createComponent(AccRptPurchaseledgerviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
