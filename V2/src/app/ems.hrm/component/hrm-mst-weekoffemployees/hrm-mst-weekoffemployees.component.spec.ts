import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmMstWeekoffemployeesComponent } from './hrm-mst-weekoffemployees.component';

describe('HrmMstWeekoffemployeesComponent', () => {
  let component: HrmMstWeekoffemployeesComponent;
  let fixture: ComponentFixture<HrmMstWeekoffemployeesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmMstWeekoffemployeesComponent]
    });
    fixture = TestBed.createComponent(HrmMstWeekoffemployeesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
