import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmMstHolidayassignemployeeComponent } from './hrm-mst-holidayassignemployee.component';

describe('HrmMstHolidayassignemployeeComponent', () => {
  let component: HrmMstHolidayassignemployeeComponent;
  let fixture: ComponentFixture<HrmMstHolidayassignemployeeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmMstHolidayassignemployeeComponent]
    });
    fixture = TestBed.createComponent(HrmMstHolidayassignemployeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
