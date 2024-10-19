import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PayMstSalarycomponentviewComponent } from './pay-mst-salarycomponentview.component';

describe('PayMstSalarycomponentviewComponent', () => {
  let component: PayMstSalarycomponentviewComponent;
  let fixture: ComponentFixture<PayMstSalarycomponentviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PayMstSalarycomponentviewComponent]
    });
    fixture = TestBed.createComponent(PayMstSalarycomponentviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
