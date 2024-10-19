import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnTelecampaignmanagersummaryScheduledComponent } from './crm-trn-telecampaignmanagersummary-scheduled.component';

describe('CrmTrnTelecampaignmanagersummaryScheduledComponent', () => {
  let component: CrmTrnTelecampaignmanagersummaryScheduledComponent;
  let fixture: ComponentFixture<CrmTrnTelecampaignmanagersummaryScheduledComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnTelecampaignmanagersummaryScheduledComponent]
    });
    fixture = TestBed.createComponent(CrmTrnTelecampaignmanagersummaryScheduledComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
