import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmRptEmployeeFormAComponent } from './hrm-rpt-employee-form-a.component';

describe('HrmRptEmployeeFormAComponent', () => {
  let component: HrmRptEmployeeFormAComponent;
  let fixture: ComponentFixture<HrmRptEmployeeFormAComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmRptEmployeeFormAComponent]
    });
    fixture = TestBed.createComponent(HrmRptEmployeeFormAComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
