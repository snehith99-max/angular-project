import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmTrnEmployeeexitrequisitionaddComponent } from './hrm-trn-employeeexitrequisitionadd.component';

describe('HrmTrnEmployeeexitrequisitionaddComponent', () => {
  let component: HrmTrnEmployeeexitrequisitionaddComponent;
  let fixture: ComponentFixture<HrmTrnEmployeeexitrequisitionaddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmTrnEmployeeexitrequisitionaddComponent]
    });
    fixture = TestBed.createComponent(HrmTrnEmployeeexitrequisitionaddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
