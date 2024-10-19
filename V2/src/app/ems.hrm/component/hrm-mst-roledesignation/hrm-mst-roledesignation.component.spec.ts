import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmMstRoledesignationComponent } from './hrm-mst-roledesignation.component';

describe('HrmMstRoledesignationComponent', () => {
  let component: HrmMstRoledesignationComponent;
  let fixture: ComponentFixture<HrmMstRoledesignationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmMstRoledesignationComponent]
    });
    fixture = TestBed.createComponent(HrmMstRoledesignationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
