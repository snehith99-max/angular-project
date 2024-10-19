import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmMstAssignedholidaygradeviewComponent } from './hrm-mst-assignedholidaygradeview.component';

describe('HrmMstAssignedholidaygradeviewComponent', () => {
  let component: HrmMstAssignedholidaygradeviewComponent;
  let fixture: ComponentFixture<HrmMstAssignedholidaygradeviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmMstAssignedholidaygradeviewComponent]
    });
    fixture = TestBed.createComponent(HrmMstAssignedholidaygradeviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
