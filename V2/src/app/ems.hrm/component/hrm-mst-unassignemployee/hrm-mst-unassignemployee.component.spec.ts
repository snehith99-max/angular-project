import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmMstUnassignemployeeComponent } from './hrm-mst-unassignemployee.component';

describe('HrmMstUnassignemployeeComponent', () => {
  let component: HrmMstUnassignemployeeComponent;
  let fixture: ComponentFixture<HrmMstUnassignemployeeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmMstUnassignemployeeComponent]
    });
    fixture = TestBed.createComponent(HrmMstUnassignemployeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
