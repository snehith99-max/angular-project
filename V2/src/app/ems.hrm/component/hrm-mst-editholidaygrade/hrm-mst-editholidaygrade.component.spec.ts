import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmMstEditholidaygradeComponent } from './hrm-mst-editholidaygrade.component';

describe('HrmMstEditholidaygradeComponent', () => {
  let component: HrmMstEditholidaygradeComponent;
  let fixture: ComponentFixture<HrmMstEditholidaygradeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmMstEditholidaygradeComponent]
    });
    fixture = TestBed.createComponent(HrmMstEditholidaygradeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
