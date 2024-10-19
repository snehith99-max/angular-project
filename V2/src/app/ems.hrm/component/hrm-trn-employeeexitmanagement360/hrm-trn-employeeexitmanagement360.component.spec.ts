import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmTrnEmployeeexitmanagement360Component } from './hrm-trn-employeeexitmanagement360.component';

describe('HrmTrnEmployeeexitmanagement360Component', () => {
  let component: HrmTrnEmployeeexitmanagement360Component;
  let fixture: ComponentFixture<HrmTrnEmployeeexitmanagement360Component>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmTrnEmployeeexitmanagement360Component]
    });
    fixture = TestBed.createComponent(HrmTrnEmployeeexitmanagement360Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
