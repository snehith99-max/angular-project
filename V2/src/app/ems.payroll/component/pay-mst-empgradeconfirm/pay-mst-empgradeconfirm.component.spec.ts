import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PayMstEmpgradeconfirmComponent } from './pay-mst-empgradeconfirm.component';

describe('PayMstEmpgradeconfirmComponent', () => {
  let component: PayMstEmpgradeconfirmComponent;
  let fixture: ComponentFixture<PayMstEmpgradeconfirmComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PayMstEmpgradeconfirmComponent]
    });
    fixture = TestBed.createComponent(PayMstEmpgradeconfirmComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
