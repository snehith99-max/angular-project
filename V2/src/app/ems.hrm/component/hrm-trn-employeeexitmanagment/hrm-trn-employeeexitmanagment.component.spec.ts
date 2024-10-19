import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmTrnEmployeeexitmanagmentComponent } from './hrm-trn-employeeexitmanagment.component';

describe('HrmTrnEmployeeexitmanagmentComponent', () => {
  let component: HrmTrnEmployeeexitmanagmentComponent;
  let fixture: ComponentFixture<HrmTrnEmployeeexitmanagmentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmTrnEmployeeexitmanagmentComponent]
    });
    fixture = TestBed.createComponent(HrmTrnEmployeeexitmanagmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
