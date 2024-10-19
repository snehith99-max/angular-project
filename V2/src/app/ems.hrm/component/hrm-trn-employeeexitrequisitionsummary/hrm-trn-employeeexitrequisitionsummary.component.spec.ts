import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmTrnEmployeeexitrequisitionsummaryComponent } from './hrm-trn-employeeexitrequisitionsummary.component';

describe('HrmTrnEmployeeexitrequisitionsummaryComponent', () => {
  let component: HrmTrnEmployeeexitrequisitionsummaryComponent;
  let fixture: ComponentFixture<HrmTrnEmployeeexitrequisitionsummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmTrnEmployeeexitrequisitionsummaryComponent]
    });
    fixture = TestBed.createComponent(HrmTrnEmployeeexitrequisitionsummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
