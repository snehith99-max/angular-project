import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PayMstTdsapprovalComponent } from './pay-mst-tdsapproval.component';

describe('PayMstTdsapprovalComponent', () => {
  let component: PayMstTdsapprovalComponent;
  let fixture: ComponentFixture<PayMstTdsapprovalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PayMstTdsapprovalComponent]
    });
    fixture = TestBed.createComponent(PayMstTdsapprovalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
