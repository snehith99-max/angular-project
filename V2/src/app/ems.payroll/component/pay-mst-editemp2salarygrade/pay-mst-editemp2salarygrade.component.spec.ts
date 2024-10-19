import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PayMstEditemp2salarygradeComponent } from './pay-mst-editemp2salarygrade.component';

describe('PayMstEditemp2salarygradeComponent', () => {
  let component: PayMstEditemp2salarygradeComponent;
  let fixture: ComponentFixture<PayMstEditemp2salarygradeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PayMstEditemp2salarygradeComponent]
    });
    fixture = TestBed.createComponent(PayMstEditemp2salarygradeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
