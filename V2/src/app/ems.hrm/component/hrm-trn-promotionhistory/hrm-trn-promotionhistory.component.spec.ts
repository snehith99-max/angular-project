import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmTrnPromotionhistoryComponent } from './hrm-trn-promotionhistory.component';

describe('HrmTrnPromotionhistoryComponent', () => {
  let component: HrmTrnPromotionhistoryComponent;
  let fixture: ComponentFixture<HrmTrnPromotionhistoryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmTrnPromotionhistoryComponent]
    });
    fixture = TestBed.createComponent(HrmTrnPromotionhistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
