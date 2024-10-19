import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnPortalordersCustomerApprovalComponent } from './smr-trn-portalorders-customer-approval.component';

describe('SmrTrnPortalordersCustomerApprovalComponent', () => {
  let component: SmrTrnPortalordersCustomerApprovalComponent;
  let fixture: ComponentFixture<SmrTrnPortalordersCustomerApprovalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnPortalordersCustomerApprovalComponent]
    });
    fixture = TestBed.createComponent(SmrTrnPortalordersCustomerApprovalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
