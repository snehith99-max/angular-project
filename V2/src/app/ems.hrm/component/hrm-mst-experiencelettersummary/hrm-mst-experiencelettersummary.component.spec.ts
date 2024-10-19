import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmMstExperiencelettersummaryComponent } from './hrm-mst-experiencelettersummary.component';

describe('HrmMstExperiencelettersummaryComponent', () => {
  let component: HrmMstExperiencelettersummaryComponent;
  let fixture: ComponentFixture<HrmMstExperiencelettersummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmMstExperiencelettersummaryComponent]
    });
    fixture = TestBed.createComponent(HrmMstExperiencelettersummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
