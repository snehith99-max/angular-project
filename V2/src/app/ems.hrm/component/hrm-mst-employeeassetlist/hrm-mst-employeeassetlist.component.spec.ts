import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmMstEmployeeassetlistComponent } from './hrm-mst-employeeassetlist.component';

describe('HrmMstEmployeeassetlistComponent', () => {
  let component: HrmMstEmployeeassetlistComponent;
  let fixture: ComponentFixture<HrmMstEmployeeassetlistComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmMstEmployeeassetlistComponent]
    });
    fixture = TestBed.createComponent(HrmMstEmployeeassetlistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
