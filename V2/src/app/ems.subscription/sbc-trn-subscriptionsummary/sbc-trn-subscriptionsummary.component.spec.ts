import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SbcTrnSubscriptionsummaryComponent } from './sbc-trn-subscriptionsummary.component';

describe('SbcTrnSubscriptionsummaryComponent', () => {
  let component: SbcTrnSubscriptionsummaryComponent;
  let fixture: ComponentFixture<SbcTrnSubscriptionsummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SbcTrnSubscriptionsummaryComponent]
    });
    fixture = TestBed.createComponent(SbcTrnSubscriptionsummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
