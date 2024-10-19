import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PayMstEmployeeAssessmentsummaryComponent } from './pay-mst-employee-assessmentsummary.component';

describe('PayMstEmployeeAssessmentsummaryComponent', () => {
  let component: PayMstEmployeeAssessmentsummaryComponent;
  let fixture: ComponentFixture<PayMstEmployeeAssessmentsummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PayMstEmployeeAssessmentsummaryComponent]
    });
    fixture = TestBed.createComponent(PayMstEmployeeAssessmentsummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
