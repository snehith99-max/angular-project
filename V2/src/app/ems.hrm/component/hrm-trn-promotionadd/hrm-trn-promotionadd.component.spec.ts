import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmTrnPromotionaddComponent } from './hrm-trn-promotionadd.component';

describe('HrmTrnPromotionaddComponent', () => {
  let component: HrmTrnPromotionaddComponent;
  let fixture: ComponentFixture<HrmTrnPromotionaddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmTrnPromotionaddComponent]
    });
    fixture = TestBed.createComponent(HrmTrnPromotionaddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
