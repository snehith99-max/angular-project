import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmMstWeekoffmanagementComponent } from './hrm-mst-weekoffmanagement.component';

describe('HrmMstWeekoffmanagementComponent', () => {
  let component: HrmMstWeekoffmanagementComponent;
  let fixture: ComponentFixture<HrmMstWeekoffmanagementComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmMstWeekoffmanagementComponent]
    });
    fixture = TestBed.createComponent(HrmMstWeekoffmanagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
