import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmMstRolegradeComponent } from './hrm-mst-rolegrade.component';

describe('HrmMstRolegradeComponent', () => {
  let component: HrmMstRolegradeComponent;
  let fixture: ComponentFixture<HrmMstRolegradeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmMstRolegradeComponent]
    });
    fixture = TestBed.createComponent(HrmMstRolegradeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
