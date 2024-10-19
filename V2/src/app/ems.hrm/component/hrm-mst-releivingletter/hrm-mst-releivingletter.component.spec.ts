import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmMstReleivingletterComponent } from './hrm-mst-releivingletter.component';

describe('HrmMstReleivingletterComponent', () => {
  let component: HrmMstReleivingletterComponent;
  let fixture: ComponentFixture<HrmMstReleivingletterComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmMstReleivingletterComponent]
    });
    fixture = TestBed.createComponent(HrmMstReleivingletterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
