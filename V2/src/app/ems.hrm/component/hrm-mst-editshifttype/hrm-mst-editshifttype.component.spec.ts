import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmMstEditshifttypeComponent } from './hrm-mst-editshifttype.component';

describe('HrmMstEditshifttypeComponent', () => {
  let component: HrmMstEditshifttypeComponent;
  let fixture: ComponentFixture<HrmMstEditshifttypeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmMstEditshifttypeComponent]
    });
    fixture = TestBed.createComponent(HrmMstEditshifttypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
