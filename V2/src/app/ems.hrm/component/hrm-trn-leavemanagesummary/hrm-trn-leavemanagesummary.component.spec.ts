import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmTrnLeavemanagesummaryComponent } from './hrm-trn-leavemanagesummary.component';

describe('HrmTrnLeavemanagesummaryComponent', () => {
  let component: HrmTrnLeavemanagesummaryComponent;
  let fixture: ComponentFixture<HrmTrnLeavemanagesummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmTrnLeavemanagesummaryComponent]
    });
    fixture = TestBed.createComponent(HrmTrnLeavemanagesummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
