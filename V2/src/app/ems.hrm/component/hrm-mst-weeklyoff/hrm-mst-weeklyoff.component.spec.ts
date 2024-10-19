import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmMstWeeklyoffComponent } from './hrm-mst-weeklyoff.component';

describe('HrmMstWeeklyoffComponent', () => {
  let component: HrmMstWeeklyoffComponent;
  let fixture: ComponentFixture<HrmMstWeeklyoffComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmMstWeeklyoffComponent]
    });
    fixture = TestBed.createComponent(HrmMstWeeklyoffComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
