import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PayMstEmployeeForm16detailsComponent } from './pay-mst-employee-form16details.component';

describe('PayMstEmployeeForm16detailsComponent', () => {
  let component: PayMstEmployeeForm16detailsComponent;
  let fixture: ComponentFixture<PayMstEmployeeForm16detailsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PayMstEmployeeForm16detailsComponent]
    });
    fixture = TestBed.createComponent(PayMstEmployeeForm16detailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
