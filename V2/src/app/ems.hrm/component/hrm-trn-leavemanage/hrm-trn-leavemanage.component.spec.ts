import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmTrnLeavemanageComponent } from './hrm-trn-leavemanage.component';

describe('HrmTrnLeavemanageComponent', () => {
  let component: HrmTrnLeavemanageComponent;
  let fixture: ComponentFixture<HrmTrnLeavemanageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmTrnLeavemanageComponent]
    });
    fixture = TestBed.createComponent(HrmTrnLeavemanageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
