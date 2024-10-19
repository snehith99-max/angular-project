import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmTrnDepromotedetailssummaryComponent } from './hrm-trn-depromotedetailssummary.component';

describe('HrmTrnDepromotedetailssummaryComponent', () => {
  let component: HrmTrnDepromotedetailssummaryComponent;
  let fixture: ComponentFixture<HrmTrnDepromotedetailssummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmTrnDepromotedetailssummaryComponent]
    });
    fixture = TestBed.createComponent(HrmTrnDepromotedetailssummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
