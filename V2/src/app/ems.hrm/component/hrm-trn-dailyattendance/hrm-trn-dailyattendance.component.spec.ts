import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmTrnDailyattendanceComponent } from './hrm-trn-dailyattendance.component';

describe('HrmTrnDailyattendanceComponent', () => {
  let component: HrmTrnDailyattendanceComponent;
  let fixture: ComponentFixture<HrmTrnDailyattendanceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmTrnDailyattendanceComponent]
    });
    fixture = TestBed.createComponent(HrmTrnDailyattendanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
