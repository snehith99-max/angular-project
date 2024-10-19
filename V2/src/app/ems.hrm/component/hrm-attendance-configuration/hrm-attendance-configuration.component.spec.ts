import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmAttendanceConfigurationComponent } from './hrm-attendance-configuration.component';

describe('HrmAttendanceConfigurationComponent', () => {
  let component: HrmAttendanceConfigurationComponent;
  let fixture: ComponentFixture<HrmAttendanceConfigurationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmAttendanceConfigurationComponent]
    });
    fixture = TestBed.createComponent(HrmAttendanceConfigurationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
