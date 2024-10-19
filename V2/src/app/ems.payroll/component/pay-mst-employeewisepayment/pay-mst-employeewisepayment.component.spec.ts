import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PayMstEmployeewisepaymentComponent } from './pay-mst-employeewisepayment.component';

describe('PayMstEmployeewisepaymentComponent', () => {
  let component: PayMstEmployeewisepaymentComponent;
  let fixture: ComponentFixture<PayMstEmployeewisepaymentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PayMstEmployeewisepaymentComponent]
    });
    fixture = TestBed.createComponent(PayMstEmployeewisepaymentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
