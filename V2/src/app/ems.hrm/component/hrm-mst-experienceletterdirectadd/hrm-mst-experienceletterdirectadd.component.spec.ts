import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmMstExperienceletterdirectaddComponent } from './hrm-mst-experienceletterdirectadd.component';

describe('HrmMstExperienceletterdirectaddComponent', () => {
  let component: HrmMstExperienceletterdirectaddComponent;
  let fixture: ComponentFixture<HrmMstExperienceletterdirectaddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmMstExperienceletterdirectaddComponent]
    });
    fixture = TestBed.createComponent(HrmMstExperienceletterdirectaddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
