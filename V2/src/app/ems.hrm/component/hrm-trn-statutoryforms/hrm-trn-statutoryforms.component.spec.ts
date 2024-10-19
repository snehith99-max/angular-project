import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmTrnStatutoryformsComponent } from './hrm-trn-statutoryforms.component';

describe('HrmTrnStatutoryformsComponent', () => {
  let component: HrmTrnStatutoryformsComponent;
  let fixture: ComponentFixture<HrmTrnStatutoryformsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmTrnStatutoryformsComponent]
    });
    fixture = TestBed.createComponent(HrmTrnStatutoryformsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
