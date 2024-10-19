import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnPortalordersCustomerComponent } from './smr-trn-portalorders-customer.component';

describe('SmrTrnPortalordersCustomerComponent', () => {
  let component: SmrTrnPortalordersCustomerComponent;
  let fixture: ComponentFixture<SmrTrnPortalordersCustomerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnPortalordersCustomerComponent]
    });
    fixture = TestBed.createComponent(SmrTrnPortalordersCustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
