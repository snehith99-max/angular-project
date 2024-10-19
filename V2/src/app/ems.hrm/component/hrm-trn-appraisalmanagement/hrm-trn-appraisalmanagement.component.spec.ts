import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmTrnAppraisalmanagementComponent } from './hrm-trn-appraisalmanagement.component';

describe('HrmTrnAppraisalmanagementComponent', () => {
  let component: HrmTrnAppraisalmanagementComponent;
  let fixture: ComponentFixture<HrmTrnAppraisalmanagementComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmTrnAppraisalmanagementComponent]
    });
    fixture = TestBed.createComponent(HrmTrnAppraisalmanagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
