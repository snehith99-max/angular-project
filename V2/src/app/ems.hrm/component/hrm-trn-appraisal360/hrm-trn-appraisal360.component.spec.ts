import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmTrnAppraisal360Component } from './hrm-trn-appraisal360.component';

describe('HrmTrnAppraisal360Component', () => {
  let component: HrmTrnAppraisal360Component;
  let fixture: ComponentFixture<HrmTrnAppraisal360Component>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmTrnAppraisal360Component]
    });
    fixture = TestBed.createComponent(HrmTrnAppraisal360Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
