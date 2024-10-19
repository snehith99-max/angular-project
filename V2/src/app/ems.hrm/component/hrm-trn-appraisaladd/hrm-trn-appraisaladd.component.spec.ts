import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmTrnAppraisaladdComponent } from './hrm-trn-appraisaladd.component';

describe('HrmTrnAppraisaladdComponent', () => {
  let component: HrmTrnAppraisaladdComponent;
  let fixture: ComponentFixture<HrmTrnAppraisaladdComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmTrnAppraisaladdComponent]
    });
    fixture = TestBed.createComponent(HrmTrnAppraisaladdComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
