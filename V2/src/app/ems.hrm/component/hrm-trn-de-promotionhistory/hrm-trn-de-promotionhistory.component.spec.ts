import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmTrnDePromotionhistoryComponent } from './hrm-trn-de-promotionhistory.component';

describe('HrmTrnDePromotionhistoryComponent', () => {
  let component: HrmTrnDePromotionhistoryComponent;
  let fixture: ComponentFixture<HrmTrnDePromotionhistoryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmTrnDePromotionhistoryComponent]
    });
    fixture = TestBed.createComponent(HrmTrnDePromotionhistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
