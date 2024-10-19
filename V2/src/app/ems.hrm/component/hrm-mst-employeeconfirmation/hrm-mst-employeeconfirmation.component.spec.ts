import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmMstEmployeeconfirmationComponent } from './hrm-mst-employeeconfirmation.component';

describe('HrmMstEmployeeconfirmationComponent', () => {
  let component: HrmMstEmployeeconfirmationComponent;
  let fixture: ComponentFixture<HrmMstEmployeeconfirmationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmMstEmployeeconfirmationComponent]
    });
    fixture = TestBed.createComponent(HrmMstEmployeeconfirmationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
