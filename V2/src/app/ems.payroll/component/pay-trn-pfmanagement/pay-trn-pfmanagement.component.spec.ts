import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PayTrnPfmanagementComponent } from './pay-trn-pfmanagement.component';

describe('PayTrnPfmanagementComponent', () => {
  let component: PayTrnPfmanagementComponent;
  let fixture: ComponentFixture<PayTrnPfmanagementComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PayTrnPfmanagementComponent]
    });
    fixture = TestBed.createComponent(PayTrnPfmanagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
