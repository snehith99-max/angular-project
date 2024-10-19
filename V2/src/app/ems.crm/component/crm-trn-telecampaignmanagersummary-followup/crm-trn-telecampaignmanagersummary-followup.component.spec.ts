import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnTelecampaignmanagersummaryFollowupComponent } from './crm-trn-telecampaignmanagersummary-followup.component';

describe('CrmTrnTelecampaignmanagersummaryFollowupComponent', () => {
  let component: CrmTrnTelecampaignmanagersummaryFollowupComponent;
  let fixture: ComponentFixture<CrmTrnTelecampaignmanagersummaryFollowupComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnTelecampaignmanagersummaryFollowupComponent]
    });
    fixture = TestBed.createComponent(CrmTrnTelecampaignmanagersummaryFollowupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
