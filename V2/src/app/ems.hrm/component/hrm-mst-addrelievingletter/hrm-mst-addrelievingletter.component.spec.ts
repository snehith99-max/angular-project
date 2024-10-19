import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmMstAddrelievingletterComponent } from './hrm-mst-addrelievingletter.component';

describe('HrmMstAddrelievingletterComponent', () => {
  let component: HrmMstAddrelievingletterComponent;
  let fixture: ComponentFixture<HrmMstAddrelievingletterComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmMstAddrelievingletterComponent]
    });
    fixture = TestBed.createComponent(HrmMstAddrelievingletterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
