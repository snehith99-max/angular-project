import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnRenewalteamsummaryComponent } from './smr-trn-renewalteamsummary.component';

describe('SmrTrnRenewalteamsummaryComponent', () => {
  let component: SmrTrnRenewalteamsummaryComponent;
  let fixture: ComponentFixture<SmrTrnRenewalteamsummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnRenewalteamsummaryComponent]
    });
    fixture = TestBed.createComponent(SmrTrnRenewalteamsummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
