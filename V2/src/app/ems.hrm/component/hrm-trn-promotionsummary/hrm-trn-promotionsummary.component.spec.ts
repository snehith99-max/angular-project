import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmTrnPromotionsummaryComponent } from './hrm-trn-promotionsummary.component';

describe('HrmTrnPromotionsummaryComponent', () => {
  let component: HrmTrnPromotionsummaryComponent;
  let fixture: ComponentFixture<HrmTrnPromotionsummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmTrnPromotionsummaryComponent]
    });
    fixture = TestBed.createComponent(HrmTrnPromotionsummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
