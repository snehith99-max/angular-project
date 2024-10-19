import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnRenewalmanagersummaryComponent } from './smr-trn-renewalmanagersummary.component';

describe('SmrTrnRenewalmanagersummaryComponent', () => {
  let component: SmrTrnRenewalmanagersummaryComponent;
  let fixture: ComponentFixture<SmrTrnRenewalmanagersummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnRenewalmanagersummaryComponent]
    });
    fixture = TestBed.createComponent(SmrTrnRenewalmanagersummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
