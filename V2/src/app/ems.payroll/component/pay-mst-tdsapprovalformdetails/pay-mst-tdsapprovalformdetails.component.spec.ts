import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PayMstTdsapprovalformdetailsComponent } from './pay-mst-tdsapprovalformdetails.component';

describe('PayMstTdsapprovalformdetailsComponent', () => {
  let component: PayMstTdsapprovalformdetailsComponent;
  let fixture: ComponentFixture<PayMstTdsapprovalformdetailsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PayMstTdsapprovalformdetailsComponent]
    });
    fixture = TestBed.createComponent(PayMstTdsapprovalformdetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
